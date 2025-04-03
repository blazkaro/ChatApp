using ChatApp.RealTimeCommunication;
using ChatApp.RealTimeCommunication.Configuration;
using ChatApp.RealTimeCommunication.Services;
using ChatApp.RealTimeCommunication.Services.Background;
using ChatApp.RealTimeCommunication.Services.Impl;
using ChatApp.RealTimeCommunication.Stores;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using StackExchange.Redis;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

var redisConfigSection = builder.Configuration.GetRequiredSection("Redis");
var auth0ConfigSection = builder.Configuration.GetRequiredSection("Auth0");

builder.Services.AddSingleton<IUserConnectionsStore, InMemoryUserConnectionStore>();
builder.Services.AddSingleton<IUserConversationsStore, InMemoryUserConversationsStore>();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAccessTokenService, AccessTokenService>();

builder.Services.AddSignalR()
    .AddStackExchangeRedis(builder.Configuration.GetConnectionString("Redis"), cfg =>
    {
        cfg.Configuration.ClientName = redisConfigSection.GetValue<string>("ClientName");
        cfg.Configuration.ChannelPrefix = RedisChannel.Literal(redisConfigSection.GetValue<string>("ChannelPrefix"));
    });

builder.Services.AddHostedService<RedisConversationEventsHandler>();

builder.Services.Configure<ApiConfig>(builder.Configuration.GetRequiredSection(nameof(ApiConfig)));

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
        cfg.TokenValidationParameters.NameClaimType = ClaimTypes.NameIdentifier;
    });

builder.Services.AddAuthorization();
builder.Services.AddHealthChecks();

builder.Services.AddCors(cfg =>
{
    cfg.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(builder.Configuration.GetRequiredSection("AllowedOrigins").Get<string[]>());
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<CommunicationHub>("");
app.MapHealthChecks("/health");

app.Run();
