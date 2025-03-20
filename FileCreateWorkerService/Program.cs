using FileCreateWorkerService;
using FileCreateWorkerService.Registrations;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddRabbitMqRegistration(builder.Configuration);

var host = builder.Build();
host.Run();
