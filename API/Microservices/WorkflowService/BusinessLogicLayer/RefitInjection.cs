using BusinessLogicLayer.ApiClients;
using BusinessLogicLayer.ApiClients.Clients;
using BusinessLogicLayer.ApiClients.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public static class RefitInjection
    {
        public static void AddRefitClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRefitClient<IBoardsServiceClient>()
                .ConfigureHttpClient(client => client
                .BaseAddress = new Uri(configuration["RefitClients:BoardsServiceClient:BaseAddress"]))
                .AddHttpMessageHandler<AuthHeaderHandler>();
        }
    }
}
