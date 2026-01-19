using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace eCommerce.OrderService.BusinessLogicLayer.Consumer;

public class ProductNameUpdateConsumer : IDisposable, IProductNameUpdateConsumer
{
    private readonly IConfiguration _configuration;
    private readonly IChannel _channel;
    private readonly IConnection _connection;
    private readonly ILogger<ProductNameUpdateConsumer> _logger;

    public ProductNameUpdateConsumer(IConfiguration configuration, ILogger<ProductNameUpdateConsumer> logger)
    {
        _configuration = configuration;
        _logger = logger;

        var hostName = _configuration["RabbitMQ_HostName"]!;
        var userName = _configuration["RabbitMQ_UserName"]!;
        var password = _configuration["RabbitMQ_Password"]!;
        var port = Convert.ToInt32(_configuration["RabbitMQ_Port"]!);

        _logger.LogInformation("Creating RabbitMQ connection and channel...");
        ConnectionFactory connectionFactory = new ConnectionFactory()
        {
            HostName = hostName,
            UserName = userName,
            Password = password,
            Port = Convert.ToInt32(port)
        };
        _logger.LogInformation("Connecting to RabbitMQ..." + connectionFactory.HostName + ":" + connectionFactory.Port);
        _connection = connectionFactory.CreateConnectionAsync()
            .GetAwaiter()
            .GetResult();

        _logger.LogInformation("Creating RabbitMQ channel...");
        _channel = _connection.CreateChannelAsync()
            .GetAwaiter()
            .GetResult();
        _logger.LogInformation("RabbitMQ connection and channel created.");
    }

    public void Consume()
    {
        string routingKey = "net9.ecomm.aks.product.update.name";
        string queueName = "orders.product.update.name.queue";

        //Create exchange
        string exchangeName = _configuration["RabbitMQ_Products_Exchange"]!;
        _channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Direct, durable: true).GetAwaiter().GetResult();

        //Create message queue
        _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null).GetAwaiter().GetResult(); //x-message-ttl | x-max-length | x-expired 

        //Bind the message to exchange
        _channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: routingKey).GetAwaiter().GetResult();

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (sender, args) =>
        {
            byte[] body = args.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);

            if (message != null)
            {
                ProductNameUpdateMessage? productNameUpdateMessage = JsonSerializer.Deserialize<ProductNameUpdateMessage>(message);

                _logger.LogInformation($"Product name updated: {productNameUpdateMessage!.ProductID}, New name: {productNameUpdateMessage.NewName}");
            }

            //await _channel.BasicConsumeAsync(queue: queueName, consumer: consumer, autoAck: true);
        };

        _channel.BasicConsumeAsync(queue: queueName, consumer: consumer, autoAck: true).GetAwaiter().GetResult();
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}