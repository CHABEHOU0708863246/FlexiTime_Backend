using FlexiTime_Backend.Domain.Configurations;

namespace FlexiTime_Backend.WebApi.Configurations
{
    public static class CorsConfig
    {
        public const string DEFAULT_POLICY = "DEFAULT_POLICY";
        public const string POLICY_ALL = "POLICY_ALL";
        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            CorsOption corsOption = new CorsOption();
            configuration.GetSection("Cors").Bind(corsOption);
            services.AddCors(options =>
            {
                options.AddPolicy(POLICY_ALL, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });

                options.AddPolicy(DEFAULT_POLICY, builder =>
                {
                    builder.WithOrigins(corsOption.Origin)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }
    }
}
