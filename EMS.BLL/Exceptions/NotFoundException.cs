using System.Net;

namespace EMS.BLL.Exceptions;

public class NotFoundException : CustomException
{
    public NotFoundException(string message)
        : base(message,
            null,
            HttpStatusCode.NotFound)
    {
    }
}