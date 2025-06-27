namespace EMS.BLL.Exceptions;

public class JwtTokenExpiredException : JwtUnauthorizedException
{
    public JwtTokenExpiredException() : base("Token is expired") { }
}