using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace eCommerce.ProductService.BusinessAccessLayer.Publisher;

public class ProductEventPublish : IProductEvent, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly dynamic _channel;
    private readonly dynamic _connection;
    private readonly ILogger<ProductEventPublish> _logger;

    public ProductEventPublish(IConfiguration configuration, ILogger<ProductEventPublish> logger)
    {
        _configuration = configuration;

        string hostName = _configuration["RabbitMQ_HostName"]!;
        string userName = _configuration["RabbitMQ_UserName"]!;
        string password = _configuration["RabbitMQ_Password"]!;
        string port = _configuration["RabbitMQ_Port"]!;
        _logger = logger;

        var connectionFactory = new ConnectionFactory
        {
            HostName = hostName,
            UserName = userName,
            Password = password,
            Port = Convert.ToInt32(port)
        };

        _logger.LogInformation("Creating RabbitMQ connection and channel...");
        // Use async factory methods if synchronous APIs are not available in the installed client
        _connection = connectionFactory
            .CreateConnectionAsync()
            .GetAwaiter()
            .GetResult();

        _channel = _connection
            .CreateChannelAsync()
            .GetAwaiter()
            .GetResult();
        _logger.LogInformation("RabbitMQ connection and channel created.");
    }

    public void Publish<T>(Dictionary<string, object> headers, T message)
    {
        string messageJson = JsonSerializer.Serialize(message);
        byte[] messageBodyInBytes = Encoding.UTF8.GetBytes(messageJson);

        string exchangeName = _configuration["RabbitMQ_Products_Exchange"]!;

        _logger.LogInformation("Publishing message to exchange {Exchange} with routing key {headers}: {MessageJson}",
            exchangeName, headers, messageJson);

        // Declare exchange and publish using synchronous IModel API
        // Use dynamic invocation to tolerate different RabbitMQ.Client API shapes in the environment
        _channel
            .ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Headers, durable: true)
            .GetAwaiter()
            .GetResult();

        var basicProperties = _channel.CreateBasicProperties();
        basicProperties.Headers = headers;

        _channel
            .BasicPublishAsync(exchange: exchangeName, routingKey: string.Empty, basicProperties: basicProperties, body: messageBodyInBytes)
            .GetAwaiter()
            .GetResult();

        _logger.LogInformation("Message published successfully.");
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}