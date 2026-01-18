using Microsoft.OpenApi;

namespace vACDMApp.Backend.Services;

public static partial class VacdmAppBackendApiExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "vACDM App API",
                Version = "v1",
                Description = "vACDMM App Api",
                Contact = new OpenApiContact
                {
                    Name = "Tim Unger",
                    Email = "tim@tim-u.me",
                    Url = new Uri("https://vacdm.tim-u.me")
                },
                License = new OpenApiLicense()
            });

            // options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            // {
            //     Name = "X-API-Key",
            //     Description = "Api-Key Authrization that may be used for external apps to communicate with the Backend",
            //     In = ParameterLocation.Header,
            //     Type = SecuritySchemeType.ApiKey
            // });
        });

        return services;
    }
}