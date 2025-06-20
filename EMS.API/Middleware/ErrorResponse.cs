namespace EMS.API.Middleware;

public record ErrorResponse
{
    public int errorCode { get; init; }
    public string errorMessage { get; init; } = string.Empty;
}