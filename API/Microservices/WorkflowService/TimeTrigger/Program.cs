using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using BusinessLogicLayer;
using BusinessLogicLayer.Services.Classes;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();
builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration.GetConnectionString("StorageAccount")));
builder.Services.AddBLLayer(builder.Configuration);

var connectionString = builder.Configuration["ServiceBus:ConnectionString"];
var topic = builder.Configuration["ServiceBus:Topic"];

if (!(string.IsNullOrEmpty(connectionString)))
{
    builder.Services.AddSingleton<ServiceBusClient>(sp =>
    {
        return new ServiceBusClient(connectionString);
    });

    builder.Services.AddSingleton<ServiceBusSender>(sp =>
    {
        var client = sp.GetRequiredService<ServiceBusClient>();
        return client.CreateSender(topic);
    });
}


builder.Services.AddScoped<AzureBusEventPublisherService>();

builder.Build().Run();


