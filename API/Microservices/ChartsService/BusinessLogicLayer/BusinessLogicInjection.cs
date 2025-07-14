using BusinessLogicLayer.Mapping;
using DataAccessLayer;
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
        }
    }
}
