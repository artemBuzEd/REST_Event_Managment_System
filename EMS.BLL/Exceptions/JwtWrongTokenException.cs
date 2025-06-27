namespace EMS.BLL.Exceptions;

public class JwtWrongTokenException : JwtUnauthorizedException
{
    public JwtWrongTokenException() : base("Wrong refresh token") { }
}