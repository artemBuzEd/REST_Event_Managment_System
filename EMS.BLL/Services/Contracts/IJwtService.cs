using EMS.BLL.DTOs.Auth;

namespace EMS.BLL.Services.Contracts;

public interface IJwtService
{
    Task<LoginResponseDTO> LoginAsync(LoginRequestDTO model);
    Task<bool> RegisterAsync(string userName, string email, string password, string firstName, string lastName);
    Task<RefreshTokenResponseDTO> RefreshTokenAsync(string ipAddress);
    Task LogoutAsync();
}