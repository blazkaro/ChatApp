using ChatApp.Messages.Configuration;
using ChatApp.Messages.Db;
using ChatApp.Messages.Services;
using ChatApp.Messages.Services.Background;
using ChatApp.Messages.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var auth0ConfigSection = builder.Configuration.GetRequiredSection("Auth0");

builder.Services.AddNpgsqlDataSource(builder.Configuration.GetConnectionString("YugabyteDB"));
builder.Services.AddSingleton<IConnectionMultiplexer>(await ConnectionMultiplexer.ConnectAsync(builder.Configuration.GetConnectionString("Redis")));

var apiConfig = new ApiConfig();
builder.Configuration.GetRequiredSection(nameof(ApiConfig)).Bind(apiConfig);

builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureRestServices(apiConfig);
builder.Services.AddScoped<IAccessTokenService, AccessTokenService>();
builder.Services.AddHostedService<SaveMessagesBackgroundService>();

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

var app = builder.Build();

await app.MigrateDbIfNotExists();

app.Run();
