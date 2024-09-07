using FlexiTime_Backend.Services.Auth;
using FlexiTime_Backend.Services.Email;
using FlexiTime_Backend.Services.Roles;
using FlexiTime_Backend.Services.Token;
using FlexiTime_Backend.Services.Users;

namespace FlexiTime_Backend.WebApi.Configurations
{
    public static class ServicesConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRoleService, RolesService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
