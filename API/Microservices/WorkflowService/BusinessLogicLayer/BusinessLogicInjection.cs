using BusinessLogicLayer.ApiClients;
using BusinessLogicLayer.Mapping;
using BusinessLogicLayer.Services.Classes;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer;
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
            services.AddDataAccessLayer(configuration);
            services.AddAutoMapper(typeof(AutomapperBLLProfile));
            services.AddScoped<IBoardManagementService, BoardManagementService>();
            services.AddScoped<IStateManagementService, StateManagementService>();
            services.AddScoped<ITicketManagementService, TicketManagementService>();
            services.AddScoped<AuthHeaderHandler>();
        }
    }
}
