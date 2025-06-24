using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EMS.BLL.DTOs.Auth;
using EMS.BLL.Services.Contracts;
using EMS.DAL.EF.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EMS.BLL.Services;

public class JwtService : IJwtService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public JwtService(UserManager<User> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO dto)
    {
        var user = await _userManager.FindByNameAsync(dto.UserName!);
        if(user == null || !await _userManager.CheckPasswordAsync(user, dto.Password!))
            throw new UnauthorizedAccessException("Invalid username or password");
        
        var key = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!);
        
        var validityMinutes = int.TryParse(_configuration["JWT:TokenValidityMins"], out var mins) ? mins : 30;
        
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(validityMinutes),
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"],
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescription);

        return new LoginResponseDTO
        {
            UserName = user.UserName,
            Email = user.Email,
            AccessToken = tokenHandler.WriteToken(token),
            ExpiresIn = validityMinutes * 60
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
}