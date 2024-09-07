using FlexiTime_Backend.Domain.Configurations;
using FlexiTime_Backend.WebApi.Configurations;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<SecurityOption>(builder.Configuration.GetSection("JwtConfig"));
builder.Services.Configure<EmailOption>(builder.Configuration.GetSection("EmailConfig"));
builder.Services.RegisterServices();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddMongoDbConfig(builder.Configuration);
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.ConfigureCors(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseCors(CorsConfig.DEFAULT_POLICY);

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseSwaggerSetup();

app.Run();