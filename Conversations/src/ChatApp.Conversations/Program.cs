using ChatApp.Conversations.Db;
using ChatApp.Conversations.Services;
using ChatApp.Conversations.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using StackExchange.Redis;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ConversationsDbContext>(cfg =>
{
    cfg.UseNpgsql(builder.Configuration.GetConnectionString("YugabyteDB"))
        .ReplaceService<IHistoryRepository, YugabyteHistoryRepository>(); // get rid of ACCESS LOCK, which is not supported by Yugabyte
});

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));
builder.Services.AddScoped<IConversationEventsPublisher, RedisConversationEventsPublisher>();

var auth0ConfigSection = builder.Configuration.GetRequiredSection("Auth0");

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
builder.Services.AddControllers();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
