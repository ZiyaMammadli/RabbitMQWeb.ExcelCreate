using FileCreateWorkerService.Services;
using RabbitMQ.Client;

namespace FileCreateWorkerService.Registrations; 

public static class RabbitMqRegistration
{
    public static void AddRabbitMqRegistration(this IServiceCollection services,IConfiguration config)
    {
        services.AddScoped(sp => new ConnectionFactory() { Uri = new Uri(config.GetConnectionString("RabbitMQ"))});
        services.AddScoped<RabbitMqClientService>();
    }
}
