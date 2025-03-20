using RabbitMQ.Client;

namespace RabbitMQWeb.ExcelCreate.Services;

public class RabbitMqClientService : IDisposable
{
    private readonly ConnectionFactory _connectionFactory;
    private IChannel _channel;
    private IConnection _connection;
    public static string ExchangeName = "ExcelDirectExchange";
    public static string QueueName = "queue-create-excel";
    public static string RouteKey = "route-create-excel";

    private readonly ILogger<RabbitMqClientService> _logger;
    public RabbitMqClientService(ConnectionFactory connectionFactory, ILogger<RabbitMqClientService> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<IChannel> Connect()
    {
        _connection = await _connectionFactory.CreateConnectionAsync();
        if (_channel is { IsOpen: true })
        {
            return _channel;
        }
        _channel = await _connection.CreateChannelAsync();
        await _channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Direct, true, false);
        await _channel.QueueDeclareAsync(QueueName, true, false, false);
        await _channel.QueueBindAsync(QueueName, ExchangeName, RouteKey);

        _logger.LogInformation("Connection is created...");

        return _channel;
    }

    public void Dispose()
    {
        _channel?.CloseAsync();
        _channel?.Dispose();
        _connection?.CloseAsync();
        _connection?.Dispose();
        _logger.LogInformation("Connection is closed...");
    }
}
