using System.Diagnostics;
using Serilog;

namespace vACDMApp.Backend.Services;

public static partial class VacdmAppBackendApiExtensions
{
    public static IApplicationBuilder UseResponseTimeLogging(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            var stopwatch = Stopwatch.StartNew();

            await next(context);

            stopwatch.Stop();
            var responseTime = stopwatch.ElapsedMilliseconds;

            if (context.Request.Path == "/rapidHotelNotificationHub")
                // Skip logging for SignalR hub requests
                return;

            switch (stopwatch.ElapsedMilliseconds)
            {
                case < 1000:
                    Log.Information("{RequestStatus} {RequestMethod} Request {RequestPath} took {ResponseTime} ms",
                        context.Response.StatusCode, context.Request.Method, context.Request.Path, responseTime);
                    return;
                case < 30_000:
                    Log.Information("Longer Response: {RequestMethod} Request {RequestPath} took {ResponseTime} ms",
                        context.Request.Method, context.Request.Path, responseTime);
                    return;
            }

            Log.Warning("Very Long Response Time: {RequestMethod} Request {RequestPath} took {ResponseTime} ms",
                context.Request.Method, context.Request.Path, responseTime);
        });

        return app;
    }
}