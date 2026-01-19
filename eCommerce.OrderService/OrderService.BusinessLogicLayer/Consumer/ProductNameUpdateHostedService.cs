using Microsoft.Extensions.Hosting;

namespace eCommerce.OrderService.BusinessLogicLayer.Consumer;

public class ProductNameUpdateHostedService : IHostedService
{
    private readonly IProductNameUpdateConsumer _productNameUpdateConsumer;

    public ProductNameUpdateHostedService(IProductNameUpdateConsumer consumer)
    {
        _productNameUpdateConsumer = consumer;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _productNameUpdateConsumer.Consume();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _productNameUpdateConsumer.Dispose();

        return Task.CompletedTask;
    }
}