using ChatApp.RealTimeCommunication.Services;
using ChatApp.RealTimeCommunication.Services.Impl.Rest;
using Polly;

namespace ChatApp.RealTimeCommunication.Configuration;

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

        services.AddHttpClient<IConversationsService, RestConversationsService>(cfg =>
        {
            cfg.BaseAddress = apiConfig.ConversationsApi.BaseUrl;
        })
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(timeoutPolicy)
            .AddPolicyHandler(circuitBreakerPolicy);

        return services;
    }
}
