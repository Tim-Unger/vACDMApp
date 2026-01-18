using System.Text.Json;
using System.Text.Json.Serialization;

namespace vACDMApp.Backend.Services;

public static partial class VacdmAppBackendApiExtensions
{
    public static IServiceCollection AddJsonSerializer(this IServiceCollection services)
    {
        // Register global JsonSerializerOptions
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = null, // PascalCase
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
        jsonOptions.Converters.Add(new JsonStringEnumConverter());

        services.AddSingleton(jsonOptions);
        
        return services;
    }
}