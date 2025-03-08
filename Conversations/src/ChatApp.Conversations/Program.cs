using ChatApp.Conversations.Db;
using ChatApp.Conversations.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

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
        cfg.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Sub;
    });

var app = builder.Build();

var conversationEndpointGroup = app.MapGroup("/");
conversationEndpointGroup.MapGet("/", GetConversationsEndpoint.GetAsync);
conversationEndpointGroup.MapPost("/", CreateConversationEndpoint.CreateAsync);
conversationEndpointGroup.RequireAuthorization(policy =>
{
    policy.RequireAuthenticatedUser();
});

var conversationInvitationsEndpointGroup = app.MapGroup("/invitations");
conversationInvitationsEndpointGroup.MapPost("/{ConversationId}", CreateConversationInvitationEndpoint.CreateAsync);
conversationInvitationsEndpointGroup.MapGet("/", GetConversationInvitationsEndpoint.GetAsync);
conversationInvitationsEndpointGroup.MapPost("/{InvitationId}/accept", AcceptConversationInvitationEndpoint.AcceptAsync);
conversationInvitationsEndpointGroup.RequireAuthorization(policy =>
{
    policy.RequireAuthenticatedUser();
});

app.Run();
