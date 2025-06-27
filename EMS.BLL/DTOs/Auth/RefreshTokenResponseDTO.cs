namespace EMS.BLL.DTOs.Auth;

public class RefreshTokenResponseDTO
{
    public string AccessToken { get; set; } = null!;
    public int ExpiresIn { get; set; }
}