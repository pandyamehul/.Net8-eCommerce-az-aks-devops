using eCommerce.OrderService.BusinessLogicLayer.DTO;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Polly.Bulkhead;


namespace eCommerce.OrderService.BusinessLogicLayer.HttpClients;

public class ProductServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProductServiceClient> _logger;
    private readonly IDistributedCache _distributedCache;

    public ProductServiceClient(HttpClient httpClient, ILogger<ProductServiceClient> logger, IDistributedCache distributedCache)
    {
        _httpClient = httpClient;
        _logger = logger;
        _distributedCache = distributedCache;
    }

    public async Task<ProductDTO?> GetProductByProductID(Guid productID)
    {
        try
        {
            //Key: product:123
            //Value: { "ProductName: "...", ...}

            // Check cache first, if not found, call Product Service API
            string cacheKey = $"product:{productID}";
            // Try to get from cache, if found return from cache
            string? cachedProduct = await _distributedCache.GetStringAsync(cacheKey);

            if (cachedProduct != null)
            {
                ProductDTO? productFromCache = JsonSerializer.Deserialize<ProductDTO>(cachedProduct);
                return productFromCache;
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"/api/products/search/product-id/{productID}");

            if (!response.IsSuccessStatusCode)
            {
                // Check for fallback response from Product Service. Handling http 503 status code.
                if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    // Fallback policy executed in Product Service, read the fallback response, return to called without caching fallback response.
                    ProductDTO? productFromFallback = await response.Content.ReadFromJsonAsync<ProductDTO>();

                    if (productFromFallback == null)
                    {
                        throw new NotImplementedException("Fallback policy was not implemented");
                    }
                    // Return the fallback response to the caller
                    return productFromFallback;
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new HttpRequestException("Bad request", null, HttpStatusCode.BadRequest);
                }
                else
                {
                    throw new HttpRequestException($"Http request failed with status code {response.StatusCode}");
                }
            }

            ProductDTO? product = await response.Content.ReadFromJsonAsync<ProductDTO>();

            if (product == null)
            {
                throw new ArgumentException("Invalid Product ID");
            }

            // Store in cache for future requests
            //Key: product:{productID}
            //Value: { "ProductName": "..", ..}
            string productJson = JsonSerializer.Serialize(product);

            // Set cache options
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
              .SetAbsoluteExpiration(TimeSpan.FromSeconds(300))
              .SetSlidingExpiration(TimeSpan.FromSeconds(100));

            string cacheKeyToWrite = $"product:{productID}";

            // Write to cache
            await _distributedCache.SetStringAsync(cacheKeyToWrite, productJson, options);

            return product;
        }
        catch (BulkheadRejectedException ex)
        {
            _logger.LogError(ex, "Bulkhead isolation blocks the request since the request queue is full");

            return new ProductDTO(
              ProductID: Guid.NewGuid(),
              ProductName: "Temporarily Unavailable (Bulkhead)",
              Category: "Temporarily Unavailable (Bulkhead)",
              UnitPrice: 0,
              QuantityInStock: 0);
        }
    }
}
