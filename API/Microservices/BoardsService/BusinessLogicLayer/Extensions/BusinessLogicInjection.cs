using Azure.Messaging.ServiceBus;
using BusinessLogicLayer.Mapping;
using BusinessLogicLayer.Services.Classes;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Initializer;
using GorilloBoards.Contracts.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Extensions
{
    public static class BusinessLogicInjection
    {
        public static void AddBLLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(new ServiceBusClient(configuration["ServiceBus:ConnectionString"]));
            services.AddSingleton<ServiceBusSender>(sp =>
            {
                var client = sp.GetRequiredService<ServiceBusClient>();
                return client.CreateSender(configuration["ServiceBus:Topic"]);
            });

            services.AddDataAccessLayer(configuration);
            services.AddAutoMapper(typeof(AutomapperBLLProfile));
            services.AddScoped<IBoardsManagmentService, BoardsManagmentService>();
            services.AddScoped<IBoardAccessService, BoardAccessService>();
            services.AddScoped<IEventPublisher, AzureBusEventPublisherService>();
            services.AddScoped<Initializer>();
        }
    }
}
