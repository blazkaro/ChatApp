using Auth0.AspNetCore.Authentication;
using ChatApp.Spa.Server.Configuration;
using ChatApp.Spa.Server.Endpoints;
using ChatApp.Spa.Server.Services;
using ChatApp.Spa.Server.Services.Impl.Rest;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Polly;
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
            var userToken = await ctx.HttpContext.GetUserAccessTokenAsync(cancellationToken: ctx.CancellationToken);
            ctx.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userToken.AccessToken);
        });
    });

builder.Services.Configure<ApiConfig>(builder.Configuration.GetRequiredSection(nameof(ApiConfig)));

var apiConfig = new ApiConfig();
builder.Configuration.GetRequiredSection(nameof(ApiConfig)).Bind(apiConfig);

builder.Services.AddDistributedMemoryCache();

const string AUTH0_CLIENT = "auth0.client";

builder.Services.AddClientCredentialsTokenManagement()
    .AddClient(AUTH0_CLIENT, client =>
    {
        client.TokenEndpoint = auth0ConfigSection["TokenEndpoint"];

        client.ClientId = auth0ConfigSection["ClientId"];
        client.ClientSecret = auth0ConfigSection["ClientSecret"];

        client.Parameters.Add("audience", apiConfig.Auth0Api.BaseUrl.AbsoluteUri);

        client.Scope = "read:users";
    });

builder.Services.AddOpenIdConnectAccessTokenManagement();

var retryPolicy = Policy<HttpResponseMessage>
    .Handle<HttpRequestException>()
    .WaitAndRetryAsync(apiConfig.CommunicationPolicy.RetryCount, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));

var timeoutPolicy = Policy
    .TimeoutAsync<HttpResponseMessage>(apiConfig.CommunicationPolicy.MaxTimeout);

var circuitBreakerPolicy = Policy<HttpResponseMessage>
    .Handle<HttpRequestException>()
    .CircuitBreakerAsync(apiConfig.CommunicationPolicy.AllowedExceptionsCountBeforeBreak, apiConfig.CommunicationPolicy.BreakDuration);

builder.Services.ConfigureHttpClientDefaults(cfg =>
{
    cfg.AddPolicyHandler(retryPolicy);
    cfg.AddPolicyHandler(timeoutPolicy);
    cfg.AddPolicyHandler(circuitBreakerPolicy);
});

builder.Services.AddHttpClient<IUserInfoService, RestUserInfoService>(cfg =>
{
    cfg.BaseAddress = apiConfig.Auth0Api.BaseUrl;
}).AddClientCredentialsTokenHandler(AUTH0_CLIENT);

builder.Services.AddHttpClient<IConversationInfoService, RestConversationInfoService>(cfg =>
{
    cfg.BaseAddress = apiConfig.ConversationsApi.BaseUrl;
}).AddUserAccessTokenHandler();

builder.Services.AddOutputCache();

var app = builder.Build();

app.UseOutputCache();

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.MapStaticAssets();

app.UseHttpsRedirection();

app.MapGet("api/conversations/{conversationId}/members", GetConversationMembers.GetAsync);
app.MapGet("api/users/current", CurrentUserInfoEndpoint.GetAsync);
app.MapGet("api/users", UserInfoEndpoint.GetAsync)
    .CacheOutput(policy =>
    {
        policy.SetVaryByQuery("namePrefix");
        policy.Expire(TimeSpan.FromSeconds(60 * 3));
        policy.Cache();
    });

app.MapReverseProxy();

app.MapFallbackToFile("/index.html")
    .RequireAuthorization();

app.Run();