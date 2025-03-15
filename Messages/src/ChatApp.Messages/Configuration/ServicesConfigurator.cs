using ChatApp.Messages.Services;
using ChatApp.Messages.Services.Impl.Rest;
using Polly;
using System.Net;

namespace ChatApp.Messages.Configuration;

public static class ServicesConfigurator
{
    public static IServiceCollection ConfigureRestServices(this IServiceCollection services, ApiConfig apiConfig)
    {
        var retryPolicy = Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(apiConfig.CommunicationPolicy.RetryCount, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));

        var timeoutPolicy = Policy
            .TimeoutAsync<HttpResponseMessage>(apiConfig.CommunicationPolicy.MaxTimeout);

        var circuitBreakerPolicy = Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .CircuitBreakerAsync(apiConfig.CommunicationPolicy.AllowedExceptionsCountBeforeBreak, apiConfig.CommunicationPolicy.BreakDuration);

        // Propagate unauthorized down to microservices
        var unauthorizedPolicy = Policy<HttpResponseMessage>
            .HandleResult(p => p.StatusCode == HttpStatusCode.Unauthorized)
            .FallbackAsync(new HttpResponseMessage(HttpStatusCode.Unauthorized));

        services.AddHttpClient<IConversationAccessCheckService, RestConversationAccessCheckService>(cfg =>
        {
            cfg.BaseAddress = apiConfig.ConversationsApi.BaseUrl;
        })
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(timeoutPolicy)
            .AddPolicyHandler(circuitBreakerPolicy)
            .AddPolicyHandler(unauthorizedPolicy);

        return services;
    }
}
