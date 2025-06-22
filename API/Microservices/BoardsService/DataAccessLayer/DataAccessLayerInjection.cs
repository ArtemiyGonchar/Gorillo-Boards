using DataAccessLayer.DatabaseContext;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public static class DataAccesLayerInjection
    {
        public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BoardsDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DbConnectionString")); //getting connection string from appsetings.json
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IBoardRepository, BoardRepository>();
            services.AddScoped<IBoardRoleRepository, BoardRoleRepository>();
        }
    }
}
