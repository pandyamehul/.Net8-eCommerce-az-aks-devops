using eCommerce.ProductsService.BusinessLogicLayer.DTO;
using eCommerce.ProductsService.BusinessLogicLayer.ServiceContracts;
using eCommerce.ProductsService.DataAccessLayer.Entities;
using eCommerce.ProductsService.DataAccessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;
using MapsterMapper;
using eCommerce.ProductService.BusinessAccessLayer.Publisher;

namespace eCommerce.ProductsService.BusinessLogicLayer.Services;

public class ProductsService : IProductsService
{
    private readonly IValidator<ProductAddRequest> _productAddRequestValidator;
    private readonly IValidator<ProductUpdateRequest> _productUpdateRequestValidator;
    private readonly IMapper _mapper;
    private readonly IProductsRepository _productsRepository;
    private readonly IProductEvent _productEvent;


    public ProductsService(
        IValidator<ProductAddRequest> productAddRequestValidator,
        IValidator<ProductUpdateRequest> productUpdateRequestValidator,
        IMapper mapper,
        IProductsRepository productsRepository,
        IProductEvent productEvent
        )
    {
        _productAddRequestValidator = productAddRequestValidator;
        _productUpdateRequestValidator = productUpdateRequestValidator;
        _mapper = mapper;
        _productsRepository = productsRepository;
        _productEvent = productEvent;
    }


    public async Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest)
    {
        if (productAddRequest == null)
        {
            throw new ArgumentNullException(nameof(productAddRequest));
        }

        //Validate the product using Fluent Validation
        ValidationResult validationResult = await _productAddRequestValidator.ValidateAsync(productAddRequest);

        // Check the validation result
        if (!validationResult.IsValid)
        {
            string errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage)); //Error1, Error2, ...
            throw new ArgumentException(errors);
        }

        //Attempt to add product
        //Map productAddRequest into 'Product' type (it invokes ProductAddRequestToProductMappingProfile)
        Product productInput = _mapper.Map<Product>(productAddRequest);
        Product? addedProduct = await _productsRepository.AddProduct(productInput);

        if (addedProduct == null)
        {
            return null;
        }

        //Map addedProduct into 'ProductRepsonse' type (it invokes ProductToProductResponseMappingProfile)
        ProductResponse addedProductResponse = _mapper.Map<ProductResponse>(addedProduct!);

        return addedProductResponse;
    }


    public async Task<bool> DeleteProduct(Guid productID)
    {
        Product? existingProduct = await _productsRepository.GetProductByCondition(temp => temp.ProductID == productID);

        if (existingProduct == null)
        {
            return false;
        }

        //Attempt to delete product
        bool isDeleted = await _productsRepository.DeleteProduct(productID);
        return isDeleted;
    }


    public async Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        Product? product = await _productsRepository.GetProductByCondition(conditionExpression);
        if (product == null)
        {
            return null;
        }

        // product is not null here
        ProductResponse productResponse = _mapper.Map<ProductResponse>(product);
        return productResponse;
    }


    public async Task<List<ProductResponse?>> GetProducts()
    {
        IEnumerable<Product?> products = await _productsRepository.GetProducts();

        // Only map non-null products
        //Invokes ProductToProductResponseMappingProfile
        IEnumerable<ProductResponse?> productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products.Where(p => p != null).ToList());
        return productResponses.ToList();
    }


    public async Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        IEnumerable<Product?> products = await _productsRepository.GetProductsByCondition(conditionExpression);

        // Only map non-null products
        IEnumerable<ProductResponse?> productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products.Where(p => p != null).ToList());
        return productResponses.ToList();
    }


    public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest)
    {
        Product? existingProduct = await _productsRepository.GetProductByCondition(temp => temp.ProductID == productUpdateRequest.ProductID);

        if (existingProduct == null)
        {
            throw new ArgumentException("Invalid Product ID");
        }

        //Validate the product using Fluent Validation
        ValidationResult validationResult = await _productUpdateRequestValidator.ValidateAsync(productUpdateRequest);

        // Check the validation result
        if (!validationResult.IsValid)
        {
            //Error1, Error2, ...
            string errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage));
            throw new ArgumentException(errors);
        }

        //Map from ProductUpdateRequest to Product type
        //Invokes ProductUpdateRequestToProductMappingProfile
        Product product = _mapper.Map<Product>(productUpdateRequest);

        //Check if product name is changed
        bool isProductNameChanged = productUpdateRequest.ProductName != existingProduct.ProductName;

        //Update product in database
        Product? updatedProduct = await _productsRepository.UpdateProduct(product);

        if (updatedProduct == null)
        {
            return null;
        }

        //Publish product.update.name message to the exchange
        if (isProductNameChanged)
        {
            // Publish event to RabbitMQ exchange about product name update
            // Note: routing key should match the one used by the consumer in OrderService
            string routingKey = "net9.ecomm.aks.product.update.name";
            var message = new ProductNameUpdateEvent(product.ProductID, product.ProductName);

            _productEvent.Publish<ProductNameUpdateEvent>(routingKey, message);
        }

        ProductResponse? updatedProductResponse = _mapper.Map<ProductResponse>(updatedProduct);

        return updatedProductResponse;
    }
}