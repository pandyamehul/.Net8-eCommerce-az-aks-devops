using eCommerce.OrderService.BusinessLogicLayer.DTO;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace eCommerce.OrderService.BusinessLogicLayer.Consumer;

public class ProductUpdateConsumer : IDisposable, IProductUpdateConsumer
{
    private readonly IConfiguration _configuration;
    private readonly IChannel _channel;
    private readonly IConnection _connection;
    private readonly ILogger<ProductUpdateConsumer> _logger;
    private readonly IDistributedCache _cache;

    public ProductUpdateConsumer(
        IConfiguration configuration,
        ILogger<ProductUpdateConsumer> logger,
        IDistributedCache cache
    )
    {
        // Initialize dependencies
        _configuration = configuration;
        _logger = logger;
        _cache = cache;

        // Read RabbitMQ settings from configuration
        var hostName = _configuration["RabbitMQ_HostName"]!;
        var userName = _configuration["RabbitMQ_UserName"]!;
        var password = _configuration["RabbitMQ_Password"]!;
        var port = Convert.ToInt32(_configuration["RabbitMQ_Port"]!);

        _logger.LogInformation("Creating RabbitMQ connection and channel...");
        // Create RabbitMQ connection factory
        ConnectionFactory connectionFactory = new ConnectionFactory()
        {
            HostName = hostName,
            UserName = userName,
            Password = password,
            Port = Convert.ToInt32(port)
        };

        _logger.LogInformation("Connecting to RabbitMQ..." + connectionFactory.HostName + ":" + connectionFactory.Port);

        // Create RabbitMQ connection and channel
        _connection = connectionFactory.CreateConnectionAsync()
            .GetAwaiter()
            .GetResult();

        _logger.LogInformation("Creating RabbitMQ channel...");
        // Create channel, make it synchronous
        _channel = _connection.CreateChannelAsync()
            .GetAwaiter()
            .GetResult();
        _logger.LogInformation("RabbitMQ connection and channel created.");
    }

    public void Consume()
    {
        //string routingKey = "net9.ecomm.aks.product.update.name";
        string queueName = "orders.product.update.queue";

        //string routingKey = "product.update.*";
        var headers = new Dictionary<string, object?>()
        {
            { "x-match", "all" },
            { "event", "product.update" },
            { "RowCount", 1 }
        };

        //Create exchange
        string exchangeName = _configuration["RabbitMQ_Products_Exchange"]!;
        _channel.ExchangeDeclareAsync(
                exchange: exchangeName, 
                type: ExchangeType.Headers, 
                durable: true)
            .GetAwaiter()
            .GetResult();

        //Create message queue
        //x-message-ttl | x-max-length | x-expired 
        _channel
            .QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null)
            .GetAwaiter()
            .GetResult();

        //Bind the message to exchange
        //_channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: routingKey).GetAwaiter().GetResult();
        _channel
            .QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: string.Empty, arguments: headers)
            .GetAwaiter()
            .GetResult();

        // Create consumer to listen for messages, process asynchronously, and update cache, auto-acknowledge
        var consumer = new AsyncEventingBasicConsumer(_channel);

        // Event handler for received messages, process the message, and update cache
        consumer.ReceivedAsync += async (sender, args) =>
        {
            byte[] body = args.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);

            if (message != null)
            {
                //ProductNameUpdateMessage? productNameUpdateMessage = JsonSerializer.Deserialize<ProductNameUpdateMessage>(message);
                ProductDTO? productDTO = JsonSerializer.Deserialize<ProductDTO>(message);

                if (productDTO != null)
                {
                    await HandleProductUpdation(productDTO);
                }
                else
                {
                    _logger.LogWarning("Received null ProductDTO in message.");
                }

                _logger.LogInformation($"Product name updated: {productDTO!.ProductID}, New name: {productDTO.ProductName}");
            }

            //await _channel.BasicConsumeAsync(queue: queueName, consumer: consumer, autoAck: true);
        };
        
        // Start consuming messages
        _channel
            .BasicConsumeAsync(queue: queueName, consumer: consumer, autoAck: true)
            .GetAwaiter()
            .GetResult();
    }


    /// <summary>
    /// Handles the product name update by updating the cache. 
    /// </summary>
    /// <param name="productDTO"></param>
    /// <returns></returns>
    private async Task HandleProductUpdation(ProductDTO productDTO)
    {
        _logger.LogInformation($"Product name updated: {productDTO.ProductID}, New name: {productDTO.ProductName}");

        string productJson = JsonSerializer.Serialize(productDTO);

        DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
          .SetAbsoluteExpiration(TimeSpan.FromSeconds(300));

        string cacheKeyToWrite = $"product:{productDTO.ProductID}";

        await _cache.SetStringAsync(cacheKeyToWrite, productJson, options);
    }


    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}