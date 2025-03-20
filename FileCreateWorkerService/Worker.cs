using ClosedXML.Excel;
using FileCreateWorkerService.Models;
using FileCreateWorkerService.Services;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQWeb.ExcelCreate.Services;
using System.Data;
using System.Text;
using System.Text.Json;

namespace FileCreateWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly RabbitMqClientService _rabbitMqClientService;
        private IChannel _channel;
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(RabbitMqClientService rabbitMqClientService, ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _rabbitMqClientService = rabbitMqClientService;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = await _rabbitMqClientService.Connect();
            await _channel.BasicQosAsync(0, 1, false);
            await base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            await _channel.BasicConsumeAsync(RabbitMqClientService.QueueName,false, consumer);
            consumer.ReceivedAsync += Consumer_ReceivedAsync;
            await Task.CompletedTask;
        }

        private async Task Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs @event)
        {
            await Task.Delay(5000);
            var createExcelMessage=JsonSerializer.Deserialize<CreateExcelMessage>(Encoding.UTF8.GetString(@event.Body.ToArray()));

            using var ms =new MemoryStream();

            var wb = new XLWorkbook();
            var ds=new DataSet();
            ds.Tables.Add(await GetTable("Products"));
            wb.Worksheets.Add(ds);
            wb.SaveAs(ms);

            MultipartFormDataContent multipartFormDataContent = new();
            multipartFormDataContent.Add(new ByteArrayContent(ms.ToArray()),"file",Guid.NewGuid().ToString()+".xlsx");
            var baseUrl = "{http://localhost:44375/api/files}?fileId="+createExcelMessage.UserFileId;
            using (var httpClient=new HttpClient())
            {
                var response = await httpClient.PostAsync(baseUrl, multipartFormDataContent);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"File (Id : {createExcelMessage.UserFileId}) was successfuly created");
                    await _channel.BasicAckAsync(@event.DeliveryTag, false);
                }
            }
        }

        private async Task<DataTable> GetTable(string tableName)
        {
            List<Product> products;
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = _serviceProvider.GetRequiredService<AdventureWorks2019Context>();

                products=await context.Products.ToListAsync();
            }
            DataTable table = new()
            {
                TableName = tableName,
            };
            table.Columns.Add("ProductId", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("ProductNumber", typeof(string));
            table.Columns.Add("Color", typeof(string));

            foreach (var item in products)
            {
                table.Rows.Add(item.ProductId,item.Name,item.ProductNumber,item.Color);
            }
            return table;
        }

    }
}
