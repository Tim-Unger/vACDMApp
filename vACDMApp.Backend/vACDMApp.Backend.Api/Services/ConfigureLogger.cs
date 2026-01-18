using Serilog;
using Serilog.Events;

namespace vACDMApp.Backend.Services;

public static partial class VacdmAppBackendApiExtensions
{
    public static ConfigureHostBuilder ConfigureLogger(this ConfigureHostBuilder builder)
    {
        LoggerConfiguration loggerConfig = new LoggerConfiguration()
            .Filter.ByExcluding(logEvent =>
                logEvent.Level == LogEventLevel.Information &&
                logEvent.Properties.ContainsKey("SourceContext") &&
                logEvent.Properties["SourceContext"].ToString()
                    .Contains(
                        "Microsoft.EntityFrameworkCore")) // Filter out EF Core Information logs since they are very verbose
            .Filter.ByExcluding(logEvent =>
                logEvent.Level == LogEventLevel.Information &&
                logEvent.Properties.ContainsKey("SourceContext") &&
                logEvent.Properties["SourceContext"].ToString()
                    .Contains(
                        "Microsoft.AspNetCore")) // Filter out Asp.Net Core Information logs since they are very verbose
#if DEBUG
            .WriteTo.Console();
#else
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Error);
#endif
        
        Log.Logger = loggerConfig.CreateLogger();

        builder.UseSerilog();

        return builder;
    }
}