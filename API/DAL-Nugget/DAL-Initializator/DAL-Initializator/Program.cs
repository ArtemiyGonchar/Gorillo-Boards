
using DAL_Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DAL_Initializator
{
    public class Program
    {
        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<GorilloBoardsDbContext>();
                context.Database.EnsureCreated();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((context, config) =>
            {
                config.AddUserSecrets<Program>();
            }).ConfigureServices((context, services) =>
            {
                services.AddDbContext<GorilloBoardsDbContext>(options =>
                    options.UseSqlServer(context.Configuration.GetConnectionString("DbConnectionString")));
            });
    }
}
