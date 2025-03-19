using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbitMQWeb.ExcelCreate.Services;

public class RabbitMQPublisher
{
    private readonly RabbitMqClientService _rabbitMqClientService;
    public RabbitMQPublisher(RabbitMqClientService rabbitMqClientService)
    {
        _rabbitMqClientService = rabbitMqClientService;
    }

    public async Task Publish(CreateExcelMessage createExcelMessage)
    {
        var channel=await _rabbitMqClientService.Connect();
        var createExcelMessageString=JsonSerializer.Serialize(createExcelMessage);
        var creatyeExcelMessageBytes=Encoding.UTF8.GetBytes(createExcelMessageString);
        var properties=new BasicProperties();
        properties.Persistent = true;
        
        await channel.BasicPublishAsync(RabbitMqClientService.ExchangeName, RabbitMqClientService.RouteKey, true, properties,creatyeExcelMessageBytes);
    }
}
