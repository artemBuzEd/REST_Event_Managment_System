namespace EMS.BLL.Exceptions;

public class JwtUnauthorizedException : UnauthorizedAccessException
{
    public JwtUnauthorizedException(string message) : base(message) { }
}