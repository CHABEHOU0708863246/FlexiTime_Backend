using Microsoft.OpenApi.Models;
using System.Reflection;

namespace FlexiTime_Backend.WebApi.Configurations
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "FlexiTime API",
                    Description = "API de l'application web de gestion digital de congés",
                });

                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "Input the JWT like: Bearer {your token}",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    }
                };
                s.AddSecurityDefinition("Bearer", securitySchema);
                s.AddSecurityRequirement(new OpenApiSecurityRequirement { { securitySchema, new[] { "Bearer" } } });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                foreach (var xmlFilePath in Directory.EnumerateFiles(AppContext.BaseDirectory, xmlFile))
                {
                    s.IncludeXmlComments(xmlFilePath, true);
                }
            });
        }

        public static void UseSwaggerSetup(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }
    }
}
