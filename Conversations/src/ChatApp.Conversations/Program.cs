using ChatApp.Conversations.DbContexts;
using ChatApp.Conversations.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ConversationsDbContext>(cfg =>
{
    cfg.UseNpgsql(builder.Configuration.GetConnectionString("YugabyteDB"));
});

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
    });

var app = builder.Build();

app.MapGroup("/")
    .MapGet("/", GetConversationsEndpoint.GetConversationsAsync)
    .RequireAuthorization(policy =>
    {
        policy.RequireAuthenticatedUser();
    });

app.Run();
