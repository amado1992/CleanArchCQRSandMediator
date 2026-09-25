using System.Net;
using System.Text.Json;
using CleanArchCQRSandMediator.Application.Common.Exceptions;

namespace CleanArchCQRSandMediator.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Interceptar el body de la respuesta
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);

            // Si ya empezó a enviarse, no se puede modificar
            if (context.Response.HasStarted)
            {
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
                return;
            }

            // Leer el body capturado
            responseBody.Seek(0, SeekOrigin.Begin);
            var bodyText = await new StreamReader(responseBody).ReadToEndAsync();

            // CRÍTICO: restaurar el stream ANTES de escribir
            context.Response.Body = originalBodyStream;

            // CRÍTICO: resetear ContentLength (ASP.NET Core lo seteó al escribir en el MemoryStream)
            context.Response.ContentLength = null;

            switch (context.Response.StatusCode)
            {
                case StatusCodes.Status400BadRequest:
                    await WriteBadRequestResponse(context, bodyText);
                    break;

                case StatusCodes.Status401Unauthorized:
                    await WriteUnauthorizedResponse(context);
                    break;

                case StatusCodes.Status403Forbidden:
                    await WriteForbiddenResponse(context);
                    break;

                default:
                    // Copiar la respuesta original tal cual
                    if (!string.IsNullOrWhiteSpace(bodyText))
                    {
                        await context.Response.WriteAsync(bodyText);
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            // Restaurar stream también aquí
            context.Response.Body = originalBodyStream;
            context.Response.ContentLength = null;

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

            case IdentityException identityEx:
                statusCode = HttpStatusCode.BadRequest; // 400
                message = identityEx.Message;
                details = identityEx.Errors.Select(e => new
                {
                    code = e.Code,
                    description = e.Code
                });
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

    private async Task WriteUnauthorizedResponse(HttpContext context)
    {
        // We didn't change the status code (it's already 401)
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = StatusCodes.Status401Unauthorized,
            title = "Unauthorized",
            detail = "Unauthenticated user or invalid ID.",
            errors = (object?)null
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }

    private async Task WriteForbiddenResponse(HttpContext context)
    {
        // We didn't change the status code (it's already 403)
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = StatusCodes.Status403Forbidden,
            title = "Forbidden",
            detail = "You do not have permission to access this resource.",
            errors = (object?)null
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }

    // ============================================
    // 400 — ProblemDetails de ASP.NET Core
    // ============================================
    private async Task WriteBadRequestResponse(HttpContext context, string originalBody)
    {
        string message = "One or more validation errors occurred.";
        object? errors = null;

        try
        {
            using var doc = JsonDocument.Parse(originalBody);
            var root = doc.RootElement;

            if (root.TryGetProperty("detail", out var detailProp) &&
                detailProp.ValueKind == JsonValueKind.String)
            {
                message = detailProp.GetString() ?? message;
            }
            else if (root.TryGetProperty("title", out var titleProp) &&
                     titleProp.ValueKind == JsonValueKind.String)
            {
                message = titleProp.GetString() ?? message;
            }

            // Transformar "errors" de objeto → array plano [{ field, message }]
            if (root.TryGetProperty("errors", out var errorsProp) &&
                errorsProp.ValueKind == JsonValueKind.Object)
            {
                var list = new List<object>();
                foreach (var prop in errorsProp.EnumerateObject())
                {
                    if (prop.Value.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var err in prop.Value.EnumerateArray())
                        {
                            list.Add(new
                            {
                                field = prop.Name,
                                message = err.GetString() ?? "Invalid value"
                            });
                        }
                    }
                    else if (prop.Value.ValueKind == JsonValueKind.String)
                    {
                        list.Add(new
                        {
                            field = prop.Name,
                            message = prop.Value.GetString() ?? "Invalid value"
                        });
                    }
                }
                errors = list;
            }
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "No se pudo parsear el body del 400 como JSON.");
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var response = new
        {
            status = StatusCodes.Status400BadRequest,
            title = "BadRequest",
            detail = message,
            errors
        };

        var json = JsonSerializer.Serialize(response, JsonOptions);
        await context.Response.WriteAsync(json);
    }
}