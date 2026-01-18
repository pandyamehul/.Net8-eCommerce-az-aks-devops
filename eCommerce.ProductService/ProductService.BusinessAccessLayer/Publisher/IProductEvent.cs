namespace eCommerce.ProductService.BusinessAccessLayer.Publisher;

public interface IProductEvent
{
    void Publish<T>(string routingKey, T message);
}