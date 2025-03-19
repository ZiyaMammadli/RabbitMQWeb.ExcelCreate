using RabbitMQ.Client;
using RabbitMQWeb.ExcelCreate.Services;

namespace RabbitMQWeb.ExcelCreate.Registrations
{
    public static class RabbitMqRegistration
    {
        public static void AddRabbitMQRegistration(this IServiceCollection services,IConfiguration config)
        {
            services.AddScoped(sp => new ConnectionFactory { Uri = new Uri( config.GetConnectionString("RabbitMQ")) });
            services.AddScoped<RabbitMqClientService>();
            services.AddScoped<RabbitMQPublisher>();
        }
    }
}
