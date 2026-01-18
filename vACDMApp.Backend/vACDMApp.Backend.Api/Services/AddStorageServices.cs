using DotNetEnv;
using StackExchange.Redis;
using vACDMApp.Backend.Core.Database;
using vACDMApp.Backend.Storage.Cache;

namespace vACDMApp.Backend.Services;

public static partial class VacdmAppBackendApiExtensions
{
    public static IServiceCollection AddStorageServices(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(client =>
        {
            Env.Load();

            var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST");
 
            return redisHost is not null
                ? ConnectionMultiplexer.Connect(redisHost)
                : throw new MissingFieldException("REDIS_HOST");
        });
        
        services.AddSingleton<IRedisClient, RedisClient>();
        services.AddDbContext<VacdmAppDbContext>();

        return services;
    }
}