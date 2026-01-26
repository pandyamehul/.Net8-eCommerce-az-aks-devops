using eCommerce.OrderService.BusinessLogicLayer.DTO;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

namespace eCommerce.OrderService.BusinessLogicLayer.Policies;

public class PollyPolicies : IPollyPolicies
{
    private readonly ILogger<PollyPolicies> _logger;

    public PollyPolicies(ILogger<PollyPolicies> logger)
    {
        _logger = logger;
    }


    /// <summary>
    /// Policy to retry failed HTTP requests, with exponential backoff between retries. 
    /// </summary>
    /// <param name="retryCount"></param>
    /// <returns></returns> <summary>
    /// Policy to retry failed HTTP requests, with exponential backoff between retries. 
    /// </summary>
    /// <param name="retryCount"></param>
    /// <returns></returns>
    public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount)
    {
        AsyncRetryPolicy<HttpResponseMessage> policy = Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(
                //Number of retries
                retryCount: retryCount,
                // Delay between retries, with exponential backoff
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                // Action to perform on each retry, such as logging
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    _logger.LogInformation($"Retry {retryAttempt} after {timespan.TotalSeconds} seconds");
                }
            );

        return policy;
    }


    /// <summary>
    /// Policy to break the circuit after a specified number of consecutive failed HTTP requests, preventing further requests for a defined duration.
    /// </summary>
    /// <param name="handledEventsAllowedBeforeBreaking"></param>
    /// <param name="durationOfBreak"></param>
    /// <returns></returns>
    public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(int handledEventsAllowedBeforeBreaking, TimeSpan durationOfBreak)
    {
        AsyncCircuitBreakerPolicy<HttpResponseMessage> policy = Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .CircuitBreakerAsync(
                //Threshold for failed requests
                handledEventsAllowedBeforeBreaking: handledEventsAllowedBeforeBreaking,
                // Waiting time to be in "Open" state, after which it will transition to "Half-Open" state
                durationOfBreak: durationOfBreak,
                // Action to perform when the circuit is opened, such as logging
                onBreak: (outcome, timespan) =>
                {
                    _logger.LogInformation($"Circuit breaker opened for {timespan.TotalMinutes} minutes due to consecutive {handledEventsAllowedBeforeBreaking} failures. The subsequent requests will be blocked");
                },
                // Action to perform when the circuit transitions to "Half-Open" state, such as logging
                onReset: () =>
                {
                    _logger.LogInformation($"Circuit breaker closed. The subsequent requests will be allowed.");
                }
            );

        return policy;
    }


    /// <summary>
    /// Policy to timeout HTTP requests that exceed a specified duration. 
    /// </summary>
    /// <param name="timeout"></param>
    /// <returns></returns>
    public IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(TimeSpan timeout)
    {
        // Define a timeout policy that cancels requests taking longer than the specified timeout duration, 
        // triggering a TimeoutRejectedException. 
        AsyncTimeoutPolicy<HttpResponseMessage> policy = Policy.TimeoutAsync<HttpResponseMessage>(timeout);

        return policy;
    }
}