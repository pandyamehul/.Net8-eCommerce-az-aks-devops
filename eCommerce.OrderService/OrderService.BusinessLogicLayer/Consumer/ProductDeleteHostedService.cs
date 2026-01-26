using Microsoft.Extensions.Hosting;

namespace eCommerce.OrderService.BusinessLogicLayer.Consumer;

public class ProductDeleteHostedService : IHostedService
{
    private readonly IProductUpdateConsumer _productDeleteConsumer;

    public ProductDeleteHostedService(IProductUpdateConsumer consumer)
    {
        _productDeleteConsumer = consumer;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _productDeleteConsumer.Consume();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _productDeleteConsumer.Dispose();

        return Task.CompletedTask;
    }
}