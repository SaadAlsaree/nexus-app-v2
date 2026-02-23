using System.Threading.RateLimiting;

namespace NEXUS.Middleware;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitingMiddleware> _logger;

    public RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        if (endpoint?.Metadata?.GetMetadata<RateLimiterAttribute>() != null)
        {
            var path = context.Request.Path.Value ?? "";
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            await _rateLimitCheck(path, ipAddress);
        }

        await _next(context);
    }

    private static Task _rateLimitCheck(string path, string ipAddress)
    {
        var rateLimiter = new SlidingWindowRateLimiter(new SlidingWindowRateLimiterOptions
        {
            AutoReplenishment = true,
            PermitLimit = 5,
            Window = TimeSpan.FromMinutes(1),
            SegmentsPerWindow = 5
        });

        var limiter = rateLimiter.CreatePartition(ipAddress);

        if (!limiter.CurrentCount >= limiter.PermitLimit)
        {
            return Task.CompletedTask;
        }

        throw new InvalidOperationException("Rate limit exceeded. Please try again later.");
    }
}

public class RateLimiterAttribute : Attribute
{
    public int PermitLimit { get; set; } = 5;
    public TimeSpan Window { get; set; } = TimeSpan.FromMinutes(1);
}
