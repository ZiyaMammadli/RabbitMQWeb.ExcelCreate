using RabbitMQ.Client;

namespace FileCreateWorkerService.Services;

public class RabbitMqClientService:IDisposable
{
    private readonly ConnectionFactory _connectionFactory;
    private IChannel _channel;
    private IConnection _connection;
    public static string QueueName = "queue-create-excel";
    
    private readonly ILogger<RabbitMqClientService> _logger;

    public RabbitMqClientService(ConnectionFactory connectionFactory, ILogger<RabbitMqClientService> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<IChannel> Connect()
    {
        _connection=await _connectionFactory.CreateConnectionAsync();
        if(_channel is { IsOpen: true })
        {
            return _channel;
        }
        _channel=await _connection.CreateChannelAsync();
        _logger.LogInformation("Connection is created ....");
        return _channel;
    }

    public void Dispose()
    {
        _connection?.CloseAsync();
        _connection?.Dispose();
        _channel?.CloseAsync();
        _channel?.Dispose();
        _logger.LogInformation("Connection is closed ....");
    }
}
