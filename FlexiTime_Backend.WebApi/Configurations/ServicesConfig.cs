using FlexiTime_Backend.Services.Auth;
using FlexiTime_Backend.Services.Email;
using FlexiTime_Backend.Services.Leaves;
using FlexiTime_Backend.Services.LeavesBalances;
using FlexiTime_Backend.Services.Roles;
using FlexiTime_Backend.Services.Token;
using FlexiTime_Backend.Services.Users;
using FlexiTime_Backend.Utilities.UpdateLeaveBalanceWithTimer;

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
            services.AddScoped<ILeaveService, LeaveService>();
            services.AddScoped<ILeaveBalanceService, LeaveBalanceService>();


            // Utiliser une durée de vie Scoped au lieu de Singleton
            services.AddScoped<LeaveBalanceUpdateService>();
            services.AddHostedService<LeaveBalanceUpdateService>();
            services.AddHostedService(provider => provider.GetRequiredService<LeaveBalanceUpdateService>());

        }
    }
}
