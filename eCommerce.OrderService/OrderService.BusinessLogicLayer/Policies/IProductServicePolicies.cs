using Polly;

namespace eCommerce.OrderService.BusinessLogicLayer.Policies;

public interface IProductServicePolicies
{
    IAsyncPolicy<HttpResponseMessage> GetFallbackPolicy();
    IAsyncPolicy<HttpResponseMessage> GetBulkheadIsolationPolicy();
}