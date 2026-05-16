namespace PersonApi.Middelewares;
public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private static Dictionary<string, (int Count, DateTime Window)> _requests = new();

    private const int LIMIT = 10; // max 10 requests
    private static readonly TimeSpan WINDOW = TimeSpan.FromMinutes(1);

    public RateLimitingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        if (!_requests.ContainsKey(ip))
        {
            _requests[ip] = (1, DateTime.UtcNow);
        }
        else
        {
            var (count, windowStart) = _requests[ip];

            if (DateTime.UtcNow - windowStart < WINDOW)
            {
                if (count >= LIMIT)
                {
                    context.Response.StatusCode = 429; // Too Many Requests
                    await context.Response.WriteAsync("Rate limit exceeded");
                    return;
                }

                _requests[ip] = (count + 1, windowStart);
            }
            else
            {
                _requests[ip] = (1, DateTime.UtcNow);
            }
        }

        await _next(context);
    }
}
