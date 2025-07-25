﻿using Azure.Messaging.ServiceBus;
using BusinessLogicLayer.ApiClients;
using BusinessLogicLayer.Mapping;
using BusinessLogicLayer.Services.Classes;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Initializer;
using GorilloBoards.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public static class BusinessLogicInjection
    {
        public static void AddBLLayer(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["ServiceBus:ConnectionString"];
            var topic = configuration["ServiceBus:Topic"];

            if (!(string.IsNullOrEmpty(connectionString)))
            {
                services.AddSingleton(new ServiceBusClient(connectionString));
                services.AddSingleton<ServiceBusSender>(sp =>
                {
                    var client = sp.GetRequiredService<ServiceBusClient>();
                    return client.CreateSender(topic);
                });
            }

            services.AddDataAccessLayer(configuration);
            services.AddAutoMapper(typeof(AutomapperBLLProfile));
            services.AddScoped<IBoardManagementService, BoardManagementService>();
            services.AddScoped<IStateManagementService, StateManagementService>();
            services.AddScoped<ITicketManagementService, TicketManagementService>();
            services.AddScoped<IFilteringService, FilteringService>();
            services.AddScoped<ITimeLogService, TimeLogService>();
            services.AddScoped<AuthHeaderHandler>();
            services.AddScoped<IEventPublisher, AzureBusEventPublisherService>();
            services.AddScoped<Initializer>();
        }
    }
}
