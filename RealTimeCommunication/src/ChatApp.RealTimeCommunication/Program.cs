using ChatApp.RealTimeCommunication;
using ChatApp.RealTimeCommunication.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var redisConfigSection = builder.Configuration.GetRequiredSection("Redis");
var auth0ConfigSection = builder.Configuration.GetRequiredSection("Auth0");

builder.Services.AddSignalR()
    .AddStackExchangeRedis(builder.Configuration.GetConnectionString("Redis"), cfg =>
    {
        cfg.Configuration.ClientName = redisConfigSection.GetValue<string>("ClientName");
        cfg.Configuration.ChannelPrefix = RedisChannel.Literal(redisConfigSection.GetValue<string>("ChannelPrefix"));
    });

var apiConfig = new ApiConfig();
builder.Configuration.GetRequiredSection(nameof(ApiConfig)).Bind(apiConfig);

builder.Services.ConfigureRestServices(apiConfig);

builder.Services.AddAuthentication(cfg =>
{
    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, cfg =>
    {
        cfg.Authority = auth0ConfigSection.GetValue<string>("Authority");
        cfg.Audience = auth0ConfigSection.GetValue<string>("Audience");
    });

builder.Services.AddAuthorization();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<CommunicationHub>("");
app.MapHealthChecks("/health");

app.Run();
