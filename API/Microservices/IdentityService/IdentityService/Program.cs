using BusinessLogicLayer;
using DataAccessLayer;
using DataAccessLayer.DatabaseContext;
using DataAccessLayer.Initializer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PresentationLayer.Extenstions;
using Serilog;
using System.Text;

namespace IdentityService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddBLLayer(builder.Configuration);
            //builder.Services.AddDataAccesLayer(builder.Configuration);
            builder.Services.AddJwtTokenProvider();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReact",
                    policy =>
                    {
                        policy   //.WithOrigins(builder.Configuration["Cors"])
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                             //.AllowCredentials()
                             .AllowAnyOrigin();
                        //.SetIsOriginAllowed(hostName => true);

                    });
            });


            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)
                    )
                };
            });


            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGenWithJWT();

            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration)
            );

            try
            {
                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    //app.UseSwagger();
                    //app.UseSwaggerUI();
                }

                using (var scope = app.Services.CreateScope())
                {
                    var serviceProvider = scope.ServiceProvider;
                    try
                    {
                        var context = serviceProvider.GetRequiredService<IdentityDbContext>();

                        var initializer = serviceProvider.GetRequiredService<Initializer>();
                        await initializer.InitializeDb(context);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                app.UseSwagger();
                app.UseSwaggerUI();


                app.UseHttpsRedirection();
                app.UseRouting();

                app.UseCors("AllowReact");

                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
