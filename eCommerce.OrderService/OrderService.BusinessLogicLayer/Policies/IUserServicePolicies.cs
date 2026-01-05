using Polly;

namespace eCommerce.OrderService.BusinessLogicLayer.Policies;

public interface IUserServicePolicies
{
    IAsyncPolicy<HttpResponseMessage> GetRetryPolicy();
    IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy();
}