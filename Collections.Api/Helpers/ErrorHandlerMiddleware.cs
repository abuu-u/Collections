using System.Net;
using System.Text.Json;
using Npgsql;

namespace Collections.Api.Helpers;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
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
        catch (Exception error)
        {
            _logger.LogError("{Error}",error.ToString());
            var response = context.Response;
            const string internalErrorMessage = "An internal server error has occured.";
            response.ContentType = "application/json";
            response.StatusCode = error switch
            {
                BadHttpRequestException => (int)HttpStatusCode.BadRequest,
                UnauthorizedException => (int)HttpStatusCode.Unauthorized,
                NotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError,
            };
            var errorMessage = response.StatusCode == (int)HttpStatusCode.InternalServerError
                ? internalErrorMessage
                : error?.Message;
            var result = JsonSerializer.Serialize(new { message = errorMessage });
            await response.WriteAsync(result);
        }
    }
}
