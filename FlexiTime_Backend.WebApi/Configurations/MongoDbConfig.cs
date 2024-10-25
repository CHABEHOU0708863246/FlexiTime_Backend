using AspNetCore.Identity.MongoDbCore.Infrastructure;
using FlexiTime_Backend.Infra.Mongo;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FlexiTime_Backend.WebApi.Configurations
{
    public static class MongoDbConfig
    {
        public static void AddMongoDbConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var dbSettings = configuration.GetSection("DATABASE").Get<DatabaseSettings>();
            services.Configure<DatabaseSettings>(options =>
            {
                options.ConnectionString = dbSettings.ConnectionString;
                options.DatabaseName = dbSettings.DatabaseName;
            });

            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = MongoClientSettings.FromConnectionString(dbSettings.ConnectionString);
                return new MongoClient(settings);
            });

            services.AddScoped(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(dbSettings.DatabaseName);
            });

            // Configuration de l'authentification (si nécessaire)
            var mongoIdentityConfig = new MongoDbIdentityConfiguration
            {
                MongoDbSettings = new MongoDbSettings
                {
                    ConnectionString = dbSettings!.ConnectionString,
                    DatabaseName = dbSettings.DatabaseName,
                },

                IdentityOptionsAction = options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;

                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 3;
                    options.Lockout.AllowedForNewUsers = true;

                    options.User.RequireUniqueEmail = true;
                }
            };

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddMongoDbStores<ApplicationUser, ApplicationRole, ObjectId>(
                    dbSettings.ConnectionString, dbSettings.DatabaseName
                )
                .AddDefaultTokenProviders()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddMongoDbStores<ApplicationUser, ApplicationRole, ObjectId>(
                    dbSettings.ConnectionString, dbSettings.DatabaseName
                );
        }
    }
}
