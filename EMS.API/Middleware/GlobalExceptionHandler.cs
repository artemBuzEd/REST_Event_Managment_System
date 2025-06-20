using System.Net;
using System.Text.Json;
using EMS.BLL.Exceptions;

namespace EMS.API.Middleware;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, errorMessage) = exception switch
        {
            NotFoundException => (HttpStatusCode.NotFound, exception.Message),
            ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
            ApplicationException => (HttpStatusCode.InternalServerError, exception.Message),
            _ => (HttpStatusCode.InternalServerError, $"Unexpected error occurred. MESSAGE: [{exception.Message}]")
        };

        var response = new ErrorResponse
        {
            errorCode = (int)statusCode,
            errorMessage = errorMessage,
        };
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        
        
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        
    }
}