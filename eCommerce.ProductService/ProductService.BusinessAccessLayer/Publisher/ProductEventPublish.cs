using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace eCommerce.ProductService.BusinessAccessLayer.Publisher;

public class ProductEventPublish : IProductEvent, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IChannel _channel;
    private readonly IConnection _connection;

    public ProductEventPublish(IConfiguration configuration)
    {
        _configuration = configuration;

        string hostName = _configuration["RabbitMQ_HostName"]!;
        string userName = _configuration["RabbitMQ_UserName"]!;
        string password = _configuration["RabbitMQ_Password"]!;
        string port = _configuration["RabbitMQ_Port"]!;

        var connectionFactory = new ConnectionFactory
        {
            HostName = hostName,
            UserName = userName,
            Password = password,
            Port = Convert.ToInt32(port)
        };

        // NEW CLIENT API: create connection and channel asynchronously, then block
        _connection = connectionFactory
            .CreateConnectionAsync()
            .GetAwaiter()
            .GetResult();

        _channel = _connection
            .CreateChannelAsync()
            .GetAwaiter()
            .GetResult();
    }

    public void Publish<T>(string routingKey, T message)
    {
        string messageJson = JsonSerializer.Serialize(message);
        byte[] messageBodyInBytes = Encoding.UTF8.GetBytes(messageJson);

        string exchangeName = _configuration["RabbitMQ_Products_Exchange"]!;

        // Adjust to your actual IChannel API if method names differ
        _channel
            .ExchangeDeclareAsync(exchangeName, ExchangeType.Direct, durable: true)
            .GetAwaiter()
            .GetResult();

        _channel
            .BasicPublishAsync(exchangeName, routingKey, body: messageBodyInBytes)
            .GetAwaiter()
            .GetResult();
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}