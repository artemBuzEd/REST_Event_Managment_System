namespace EMS.BLL.DTOs.Auth;

public class LoginResponseDTO
{
    
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? AccessToken { get; set; }
    public int ExpiresIn { get; set; }
}