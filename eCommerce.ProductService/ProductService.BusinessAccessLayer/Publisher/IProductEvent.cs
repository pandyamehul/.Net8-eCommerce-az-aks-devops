namespace eCommerce.ProductService.BusinessAccessLayer.Publisher;

public interface IProductEvent
{
    void Publish<T>(Dictionary<string, object> headers, T message);
}