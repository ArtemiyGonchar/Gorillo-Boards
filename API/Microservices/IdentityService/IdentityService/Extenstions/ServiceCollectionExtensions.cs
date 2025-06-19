using Microsoft.OpenApi.Models;

namespace PresentationLayer.Extenstions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerGenWithJWT(this IServiceCollection services)
        {
            services.AddSwaggerGen(o =>
            {
                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                o.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            return services;
        }

        public static IServiceCollection AddJwtTokenProvider(this IServiceCollection services)
        {
            services.AddScoped<JwtTokenProvider>();
            return services;
        }
    }
}
