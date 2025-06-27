namespace EMS.BLL.Exceptions;

public class JwtMissingTokenException : JwtUnauthorizedException
{
    public JwtMissingTokenException() : base("Token is missing") { }
}