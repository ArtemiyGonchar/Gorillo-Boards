
using BusinessLogicLayer;
using DataAccessLayer.DatabaseContext;
using DataAccessLayer.Initializer;
using Microsoft.IdentityModel.Tokens;
using PresentationLayer.Extensions;
using PresentationLayer.Hubs;
using Serilog;
using System.Text;

namespace WorkflowService
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
            //builder.Services.AddDataAccessLayer(builder.Configuration);

            builder.Services.AddHttpContextAccessor(); //&&&
            builder.Services.AddRefitClients(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReact",
                    policy =>
                    {
                        policy.SetIsOriginAllowed(_ => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials();

                        //policy.WithOrigins("https://gorillo-boards.azurewebsites.net/", "https://workflowservicepl.azurewebsites.net/")
                        //    .AllowAnyHeader()
                        //    .AllowAnyMethod()
                        //    .AllowCredentials();
                        //.AllowAnyOrigin();
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
            /*
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReact",
                    policy =>
                    {
                        policy.WithOrigins(builder.Configuration["Cors"])
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });*/


            builder.Services.AddSignalR();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGenWithJWT();

            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration)
            );

            var app = builder.Build();
            /*
            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var context = serviceProvider.GetRequiredService<WorkflowDbContext>();

                    var initializer = serviceProvider.GetRequiredService<Initializer>();
                    await initializer.InitializeDb(context);
                }
                catch (Exception)
                {
                    throw;
                }
            }*/

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseSwagger();
            app.UseSwaggerUI();
            //app.MapHub<WorkflowHub>("/workflowhub"); //??
            app.UseCors("AllowReact");
            app.MapHub<WorkflowHub>("/workflowhub").RequireCors("AllowReact"); //??
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            //app.MapHub<WorkflowHub>("/workflowhub");
            app.Run();
        }
    }
}
