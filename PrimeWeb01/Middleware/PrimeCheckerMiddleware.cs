using System.Globalization;

namespace PrimeWeb01.Middleware;

public class PrimeCheckerMiddleware
{
    private readonly PrimeCheckerOptions _options;
    private readonly PrimeService _primeService;
    private readonly ILogger<PrimeCheckerMiddleware> _logger;

    public PrimeCheckerMiddleware(
        RequestDelegate next,
        PrimeCheckerOptions options,
        PrimeService primeService,
        ILogger<PrimeCheckerMiddleware> logger)
    {
        ArgumentNullException.ThrowIfNull(next);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(primeService);
        ArgumentNullException.ThrowIfNull(logger);
        if (!options.Path.HasValue)
        {
            throw new ArgumentException("The prime-check path must not be empty.", nameof(options));
        }

        _options = options;
        _primeService = primeService;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.ContentType = "text/plain; charset=utf-8";
        if (context.Request.Path != _options.Path)
        {
            await context.Response.WriteAsync(
                $"Hello World! To check if a number is prime, provide URL of the form {_options.Path}?5");
            return;
        }

        string query = context.Request.QueryString.Value ?? string.Empty;
        if (query.Length <= 1 ||
            !int.TryParse(query.AsSpan(1), NumberStyles.Integer, CultureInfo.InvariantCulture, out int candidate))
        {
            _logger.LogWarning("Prime-check request did not contain a valid Int32 query.");
            await context.Response.WriteAsync($"Pass in a number to check in the form {_options.Path}?5");
            return;
        }

        string result = _primeService.IsPrime(candidate) ? "prime!" : "NOT prime!";
        await context.Response.WriteAsync($"{candidate.ToString(CultureInfo.InvariantCulture)} is {result}");
    }
}
