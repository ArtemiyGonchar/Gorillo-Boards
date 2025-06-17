using BusinessLogicLayer.Mapping;
using BusinessLogicLayer.Services.Classes;
using BusinessLogicLayer.Services.Interfaces;
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
        public static void AddBLLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutomapperBLLProfile));
            services.AddScoped<IUserService, UserService>();
        }
    }
}
