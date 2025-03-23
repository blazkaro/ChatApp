using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net.Http.Headers;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

var auth0ConfigSection = builder.Configuration.GetRequiredSection("Auth0");
builder.Services.AddAuth0WebAppAuthentication(Auth0Constants.AuthenticationScheme, cfg =>
{
    cfg.Domain = auth0ConfigSection["Domain"];
    cfg.ClientId = auth0ConfigSection["ClientId"];
    cfg.ClientSecret = auth0ConfigSection["ClientSecret"];

    cfg.Scope = "openid profile offline_access";
    cfg.ResponseType = OpenIdConnectResponseType.Code;
}).WithAccessToken(cfg =>
{
    cfg.UseRefreshTokens = true;
    cfg.Audience = "chatapp-api";
});

builder.Services.AddAuthentication(cfg =>
{
    cfg.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    cfg.DefaultChallengeScheme = Auth0Constants.AuthenticationScheme;
});

builder.Services.AddAuthorization();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetRequiredSection("ReverseProxy"))
    .AddTransforms(builder =>
    {
        builder.AddRequestTransform(async (ctx) =>
        {
            var accessToken = await ctx.HttpContext.GetTokenAsync("access_token");
            ctx.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        });
    });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.MapStaticAssets();

app.UseHttpsRedirection();

app.MapFallbackToFile("/index.html");

app.MapReverseProxy();

app.Run();