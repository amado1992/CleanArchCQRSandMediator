using System.Globalization;

public class LanguageMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LanguageMiddleware> _logger;

    public LanguageMiddleware(RequestDelegate next, ILogger<LanguageMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var culture = CultureInfo.CurrentUICulture.Name;
        _logger.LogInformation("Language of the application: {Culture}", culture);

        await _next(context);
    }
}