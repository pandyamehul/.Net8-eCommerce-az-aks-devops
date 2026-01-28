using Microsoft.Extensions.Logging;
using Polly;
using Polly.Wrap;


namespace eCommerce.OrderService.BusinessLogicLayer.Policies;

public class UserServicePolicies : IUserServicePolicies
{
    private readonly ILogger<UserServicePolicies> _logger;
    private readonly IPollyPolicies _pollyPolicies;

    public UserServicePolicies(ILogger<UserServicePolicies> logger, IPollyPolicies pollyPolicies)
    {
        _logger = logger;
        _pollyPolicies = pollyPolicies;
    }


    // public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    // {
    //     AsyncRetryPolicy<HttpResponseMessage> policy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
    //         .WaitAndRetryAsync(
    //             retryCount: 5, //Number of retries
    //             sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // Delay between retries
    //             onRetry: (outcome, timespan, retryAttempt, context) =>
    //             {
    //                 _logger.LogInformation($"Retry {retryAttempt} after {timespan.TotalSeconds} seconds");
    //             }
    //         );

    //     return policy;
    // }


    // public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    // {
    //     AsyncCircuitBreakerPolicy<HttpResponseMessage> policy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
    //         .CircuitBreakerAsync(
    //             handledEventsAllowedBeforeBreaking: 3, //Number of retries
    //             durationOfBreak: TimeSpan.FromMinutes(2), // Delay between retries
    //             onBreak: (outcome, timespan) =>
    //             {
    //                 _logger.LogInformation($"Circuit breaker opened for {timespan.TotalMinutes} minutes due to consecutive 3 failures. The subsequent requests will be blocked");
    //             },
    //             onReset: () =>
    //             {
    //                 _logger.LogInformation($"Circuit breaker closed. The subsequent requests will be allowed.");
    //             }
    //         );

    //     return policy;
    // }

    // public IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
    // {
    //     AsyncTimeoutPolicy<HttpResponseMessage> policy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromMilliseconds(1500));

    //     return policy;
    // }

    public IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy()
    {
        // Retry policy - 5 retries with exponential backoff
        var retryPolicy = _pollyPolicies.GetRetryPolicy(5);

        // Circuit Breaker policy - Break circuit after 10 consecutive failures for 30 seconds
        var circuitBreakerPolicy = _pollyPolicies.GetCircuitBreakerPolicy(10, TimeSpan.FromSeconds(30));

        // Timeout policy - Timeout after 5 seconds
        var timeoutPolicy = _pollyPolicies.GetTimeoutPolicy(TimeSpan.FromSeconds(5));

        // Combine policies using PolicyWrap
        AsyncPolicyWrap<HttpResponseMessage> wrappedPolicy = Policy.WrapAsync(
            retryPolicy,
            circuitBreakerPolicy,
            timeoutPolicy
        );

        return wrappedPolicy;
    }
}