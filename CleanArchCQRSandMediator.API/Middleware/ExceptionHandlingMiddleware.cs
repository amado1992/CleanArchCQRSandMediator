using System.Net;
using System.Text.Json;
using CleanArchCQRSandMediator.Application.Common.Exceptions;

namespace CleanArchCQRSandMediator.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode;
        string message;
        object? details = null;

        switch (exception)
        {
            case FluentValidation.ValidationException validationEx:
                statusCode = HttpStatusCode.BadRequest; // 400
                message = "Validation error";
                details = validationEx.Errors;
                break;

            case UnauthorizedException:
                statusCode = HttpStatusCode.Unauthorized; // 401
                message = exception.Message;
                break;

            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized; // 401
                message = exception.Message;
                break;

            case ForbiddenException:
                statusCode = HttpStatusCode.Forbidden; // 403
                message = exception.Message;
                break;

            case NotFoundException:
                statusCode = HttpStatusCode.NotFound; // 404
                message = exception.Message;
                break;

            case ConflictException:
                statusCode = HttpStatusCode.Conflict; // 409
                message = exception.Message;
                break;

            default:
                statusCode = HttpStatusCode.InternalServerError; // 500
                message = "An unexpected error occurred.";
                _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
                break;
        }

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = (int)statusCode,
            title = statusCode.ToString(),
            detail = message,
            errors = details
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}