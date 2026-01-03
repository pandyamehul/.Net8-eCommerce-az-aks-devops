
using eCommerce.OrderService.BusinessLogicLayer.DTO;
using eCommerce.OrderService.BusinessLogicLayer.HttpClients;
using eCommerce.OrderService.BusinessLogicLayer.ServiceContracts;
using eCommerce.OrderService.DataAccessLayer.Entities;
using eCommerce.OrderService.DataAccessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace eCommerce.OrderService.BusinessLogicLayer.Services;

public class OrdersService : IOrdersService
{
    // Validators
    private readonly IValidator<OrderAddRequest> _orderAddRequestValidator;
    private readonly IValidator<OrderItemAddRequest> _orderItemAddRequestValidator;
    private readonly IValidator<OrderUpdateRequest> _orderUpdateRequestValidator;
    private readonly IValidator<OrderItemUpdateRequest> _orderItemUpdateRequestValidator;
    // Mapper and Repository
    private readonly IMapper _mapper;
    private IOrdersRepository _ordersRepository;
    // Microservice Http Clients
    private UserServiceClient _userServiceClient;
    private ProductServiceClient _productServiceClient;
    private readonly ILogger<OrdersService> _logger;

    public OrdersService(
        IMapper mapper,
        IOrdersRepository ordersRepository,
        IValidator<OrderAddRequest> orderAddRequestValidator,
        IValidator<OrderItemAddRequest> orderItemAddRequestValidator,
        IValidator<OrderUpdateRequest> orderUpdateRequestValidator,
        IValidator<OrderItemUpdateRequest> orderItemUpdateRequestValidator,
        UserServiceClient userServiceClient,
        ProductServiceClient productServiceClient,
        ILogger<OrdersService> logger
    )
    {
        _mapper = mapper;
        _ordersRepository = ordersRepository;

        _orderAddRequestValidator = orderAddRequestValidator;
        _orderItemAddRequestValidator = orderItemAddRequestValidator;
        _orderUpdateRequestValidator = orderUpdateRequestValidator;
        _orderItemUpdateRequestValidator = orderItemUpdateRequestValidator;

        _userServiceClient = userServiceClient;
        _productServiceClient = productServiceClient;
        _logger = logger;
    }


    public async Task<OrderResponse?> AddOrder(OrderAddRequest orderAddRequest)
    {
        //Check for null parameter
        if (orderAddRequest == null)
        {
            throw new ArgumentNullException(nameof(orderAddRequest));
        }


        //Validate OrderAddRequest using Fluent Validations
        ValidationResult orderAddRequestValidationResult = await _orderAddRequestValidator.ValidateAsync(orderAddRequest);
        if (!orderAddRequestValidationResult.IsValid)
        {
            string errors = string.Join(", ", orderAddRequestValidationResult.Errors.Select(temp => temp.ErrorMessage));
            throw new ArgumentException(errors);
        }

        //Validate order items using Fluent Validation
        foreach (OrderItemAddRequest orderItemAddRequest in orderAddRequest.OrderItems)
        {
            ValidationResult orderItemAddRequestValidationResult = await _orderItemAddRequestValidator.ValidateAsync(orderItemAddRequest);

            if (!orderItemAddRequestValidationResult.IsValid)
            {
                string errors = string.Join(", ", orderItemAddRequestValidationResult.Errors.Select(temp => temp.ErrorMessage));
                throw new ArgumentException(errors);
            }

            //TO DO: Add logic for checking if ProductID exists in Products microservice
            ProductDTO? product = await _productServiceClient.GetProductByProductID(orderItemAddRequest.ProductID);
            if (product == null)
            {
                throw new ArgumentException("Invalid Product ID");
            }
        }

        //TO DO: Add logic for checking if UserID exists in Users microservice
        UserDTO? user = await _userServiceClient.GetUserByUserID(orderAddRequest.UserID);
        if (user == null)
        {
            throw new ArgumentException("Invalid User ID");
        }

        //Convert data from OrderAddRequest to Order
        //Map OrderAddRequest to 'Order' type (it invokes OrderAddRequestToOrderMappingProfile class)
        Order orderInput = _mapper.Map<Order>(orderAddRequest);

        //Generate values
        foreach (OrderItem orderItem in orderInput.OrderItems)
        {
            orderItem.TotalPrice = orderItem.Quantity * orderItem.UnitPrice;
        }
        orderInput.TotalBill = orderInput.OrderItems.Sum(temp => temp.TotalPrice);


        //Invoke repository
        Order? addedOrder = await _ordersRepository.AddOrder(orderInput);

        if (addedOrder == null)
        {
            return null;
        }

        //Map addedOrder ('Order' type) into 'OrderResponse' type (it invokes OrderToOrderResponseMappingProfile).
        OrderResponse addedOrderResponse = _mapper.Map<OrderResponse>(addedOrder);

        return addedOrderResponse;
    }



    public async Task<OrderResponse?> UpdateOrder(OrderUpdateRequest orderUpdateRequest)
    {
        //Check for null parameter
        if (orderUpdateRequest == null)
        {
            throw new ArgumentNullException(nameof(orderUpdateRequest));
        }


        //Validate OrderAddRequest using Fluent Validations
        ValidationResult orderUpdateRequestValidationResult = await _orderUpdateRequestValidator.ValidateAsync(orderUpdateRequest);
        if (!orderUpdateRequestValidationResult.IsValid)
        {
            string errors = string.Join(", ", orderUpdateRequestValidationResult.Errors.Select(temp => temp.ErrorMessage));
            throw new ArgumentException(errors);
        }

        //Validate order items using Fluent Validation
        foreach (OrderItemUpdateRequest orderItemUpdateRequest in orderUpdateRequest.OrderItems)
        {
            ValidationResult orderItemUpdateRequestValidationResult = await _orderItemUpdateRequestValidator.ValidateAsync(orderItemUpdateRequest);

            if (!orderItemUpdateRequestValidationResult.IsValid)
            {
                string errors = string.Join(", ", orderItemUpdateRequestValidationResult.Errors.Select(temp => temp.ErrorMessage));
                throw new ArgumentException(errors);
            }

            //TO DO: Add logic for checking if ProductID exists in Products microservice
            ProductDTO? product = await _productServiceClient.GetProductByProductID(orderItemUpdateRequest.ProductID);
            if (product == null)
            {
                throw new ArgumentException("Invalid Product ID");
            }
        }

        //TO DO: Add logic for checking if UserID exists in Users microservice
        UserDTO? user = await _userServiceClient.GetUserByUserID(orderUpdateRequest.UserID);
        if (user == null)
        {
            throw new ArgumentException("Invalid User ID");
        }

        //Convert data from OrderUpdateRequest to Order
        Order orderInput = _mapper.Map<Order>(orderUpdateRequest); //Map OrderUpdateRequest to 'Order' type (it invokes OrderUpdateRequestToOrderMappingProfile class)

        //Generate values
        foreach (OrderItem orderItem in orderInput.OrderItems)
        {
            orderItem.TotalPrice = orderItem.Quantity * orderItem.UnitPrice;
        }
        orderInput.TotalBill = orderInput.OrderItems.Sum(temp => temp.TotalPrice);


        //Invoke repository
        Order? updatedOrder = await _ordersRepository.UpdateOrder(orderInput);

        if (updatedOrder == null)
        {
            return null;
        }

        OrderResponse addedOrderResponse = _mapper.Map<OrderResponse>(updatedOrder); //Map addedOrder ('Order' type) into 'OrderResponse' type (it invokes OrderToOrderResponseMappingProfile).

        return addedOrderResponse;
    }

    public async Task<bool> DeleteOrder(Guid orderID)
    {
        FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(temp => temp.OrderID, orderID);
        Order? existingOrder = await _ordersRepository.GetOrderByCondition(filter);

        if (existingOrder == null)
        {
            return false;
        }


        bool isDeleted = await _ordersRepository.DeleteOrder(orderID);
        return isDeleted;
    }


    public async Task<OrderResponse?> GetOrderByCondition(FilterDefinition<Order> filter)
    {
        Order? order = await _ordersRepository.GetOrderByCondition(filter);
        if (order == null)
            return null;

        OrderResponse orderResponse = _mapper.Map<OrderResponse>(order);

        //TO DO: Load ProductName and Category in each OrderItem
        if (orderResponse != null)
        {

            foreach (OrderItemResponse orderItemResponse in orderResponse.OrderItems)
            {
                ProductDTO? productDTO = await _productServiceClient.GetProductByProductID(orderItemResponse.ProductID);

                if (productDTO == null)
                    continue;

                _mapper.Map<ProductDTO, OrderItemResponse>(productDTO, orderItemResponse);
            }
        }

        return orderResponse;
    }


    public async Task<List<OrderResponse?>> GetOrdersByCondition(FilterDefinition<Order> filter)
    {
        // Invoke repository to get orders data based on filter
        IEnumerable<Order?> orders = await _ordersRepository.GetOrdersByCondition(filter);
        // Map 'Order' type to 'OrderResponse' type (using Mapster) and return as List<OrderResponse> type data to caller
        IEnumerable<OrderResponse?> orderResponses = _mapper.Map<IEnumerable<OrderResponse>>(orders);

        //TO DO: Load ProductName and Category in each OrderItem
        var orderResponsesList = new List<OrderResponse>();
        foreach (OrderResponse? orderResponse in orderResponses)
        {
            if (orderResponse == null)
                continue;

            foreach (OrderItemResponse orderItemResponse in orderResponse.OrderItems)
            {
                ProductDTO? productDTO = await _productServiceClient.GetProductByProductID(orderItemResponse.ProductID);

                if (productDTO == null)
                    continue;

                _mapper.Map<ProductDTO, OrderItemResponse>(productDTO, orderItemResponse);
            }
            orderResponsesList.Add(orderResponse);
        }
        orderResponses = orderResponsesList;

        // Convert IEnumerable to List and return
        return orderResponses.ToList();
    }


    public async Task<List<OrderResponse?>> GetOrders()
    {
        // Invoke repository to get all orders data
        IEnumerable<Order?> orders = await _ordersRepository.GetOrders();
        // Map 'Order' type to 'OrderResponse' type (using Mapster) and return as List<OrderResponse> type data to caller
        IEnumerable<OrderResponse?> orderResponses = _mapper.Map<IEnumerable<OrderResponse>>(orders);

        //TO DO: Load ProductName and Category in each OrderItem
        var orderResponsesList = new List<OrderResponse>();
        foreach (OrderResponse? orderResponse in orderResponses)
        {
            if (orderResponse == null)
                continue;

            foreach (OrderItemResponse orderItemResponse in orderResponse.OrderItems)
            {
                ProductDTO? productDTO = await _productServiceClient.GetProductByProductID(orderItemResponse.ProductID);

                if (productDTO == null)
                    continue;

                _mapper.Map<ProductDTO, OrderItemResponse>(productDTO, orderItemResponse);
            }
            orderResponsesList.Add(orderResponse);
        }
        orderResponses = orderResponsesList;

        // Convert IEnumerable to List and return
        return orderResponses.ToList();
    }
}