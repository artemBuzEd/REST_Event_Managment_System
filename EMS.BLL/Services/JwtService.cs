using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EMS.BLL.DTOs.Auth;
using EMS.BLL.Exceptions;
using EMS.BLL.Services.Contracts;
using EMS.DAL.EF.Entities;
using EMS.DAL.EF.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EMS.BLL.Services;

public class JwtService : IJwtService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtService(UserManager<User> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO dto)
    {
        var user = await _userManager.FindByNameAsync(dto.UserName!);
        if(user == null || !await _userManager.CheckPasswordAsync(user, dto.Password!))
            throw new JwtUnauthorizedException("Invalid username or password");
        
        
        var validityMinutes = int.TryParse(_configuration["JWT:TokenValidityMins"], out var mins) ? mins : 30;
        
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = GenerateJwtToken(user, roles);
        var expiresIn = 30 * 60;
        
        var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "No ip address";
        
        var refreshToken = GenerateRefreshToken(user.Id,ipAddress);
        user.RefreshTokens.Add(refreshToken);
        
        CleanOldRefreshTokens(user);
        await _userManager.UpdateAsync(user);
        
        SetRefreshTokenCookies(refreshToken.Token, refreshToken.Expires);

        return new LoginResponseDTO()
        {
            UserName = user.UserName,
            Email = user.Email,
            AccessToken = accessToken,
            ExpiresIn = expiresIn,
        };
    }

    public async Task<bool> RegisterAsync(string userName, string email, string password, string firstName, string lastName)
    {
        if(await _userManager.FindByNameAsync(userName) != null || await _userManager.FindByEmailAsync(email) != null)
            throw new ValidationException("User creation failed. User exists!");

        var user = new Organizer()
        {
            UserName = userName,
            Email = email,
            EmailConfirmed = true,
            Name = firstName
        };
        
        var result = await _userManager.CreateAsync(user, password);
        if(result.Succeeded)
            await _userManager.AddToRoleAsync(user, "Organizer");
        
        return result.Succeeded;
    }

    public async Task<RefreshTokenResponseDTO> RefreshTokenAsync(string ipAddress)
    {
        var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];
        if(string.IsNullOrEmpty(refreshToken))
            throw new JwtUnauthorizedException("Refresh token is empty");
        
        var user = await _userManager.Users.Include(u => u.RefreshTokens)
            .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));

        if (user == null)
            throw new JwtWrongTokenException();
        
        var token = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken) ?? throw new JwtWrongTokenException();
        
        if(!token.IsActive)
            throw new JwtWrongTokenException();
        
        var newRefreshToken = GenerateRefreshToken(user.Id,ipAddress);
        token.Revoked = DateTime.UtcNow;
        token.RevokedByIp = ipAddress;
        token.ReplacedByToken = newRefreshToken.Token;
        user.RefreshTokens.Add(newRefreshToken);
        
        CleanOldRefreshTokens(user);
        await _userManager.UpdateAsync(user);
        
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = GenerateJwtToken(user, roles);
        
        SetRefreshTokenCookies(newRefreshToken.Token, newRefreshToken.Expires);

        return new RefreshTokenResponseDTO
        {
            AccessToken = accessToken,
            ExpiresIn = GetTokenValiditySeconds()
        };
    }

    public async Task LogoutAsync()
    {
        var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];
        if(string.IsNullOrEmpty(refreshToken))
            throw new JwtMissingTokenException();
        
        var user = await _userManager.Users.Include(u => u.RefreshTokens)
            .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
        
        if(user == null)
            throw new JwtWrongTokenException();

        var token = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);
        if (token is not {IsActive: true})
            throw new JwtTokenExpiredException();
        
        token.Revoked = DateTime.UtcNow;
        token.RevokedByIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "No ip address";
        
        await _userManager.UpdateAsync(user);
        
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete("refreshToken");
    }
    private string GenerateJwtToken(User user, IList<string> roles)
    {
        var claims = GenerateClaims(user, roles);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(30),
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"],
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!)),
                    SecurityAlgorithms.HmacSha512Signature)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static IEnumerable<Claim> GenerateClaims(User user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        return claims;
    }

    private RefreshToken GenerateRefreshToken(string userId, string ipAddress)
    {
        var randomNumber = new byte[64];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(randomNumber);

        return new RefreshToken
        {
            UserId = userId,
            Token = Convert.ToBase64String(randomNumber),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };
        
    }

    private void CleanOldRefreshTokens(User user, int maxActiveTokens = 5)
    {
        var activeTokens = user.RefreshTokens
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.Created)
            .ToList();
        
        if(activeTokens.Count <= maxActiveTokens)
            return;
        
        var tokensToRemove = activeTokens.Skip(maxActiveTokens).ToList();
        foreach (var token in tokensToRemove)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = "auto-cleanup";
        }
    }

    private void SetRefreshTokenCookies(string token, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = expires
        };
        
        _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", token, cookieOptions);
    }
    
    private int GetTokenValiditySeconds() => 30 * 60;
}