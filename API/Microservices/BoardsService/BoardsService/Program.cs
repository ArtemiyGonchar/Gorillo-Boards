using BusinessLogicLayer.Extensions;
using DataAccessLayer;
using DataAccessLayer.DatabaseContext;
using DataAccessLayer.Initializer;
using Microsoft.IdentityModel.Tokens;
using PresentationLayer.Extensions;
using Serilog;
using System.Text;
namespace BoardsService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //builder.Services.AddDataAccessLayer(builder.Configuration);
            builder.Services.AddBLLayer(builder.Configuration);

            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGenWithJWT();

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
            });
            */
            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration)
            );

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var context = serviceProvider.GetRequiredService<BoardsDbContext>();

                    var initializer = serviceProvider.GetRequiredService<Initializer>();
                    await initializer.InitializeDb(context);
                }
                catch (Exception)
                {
                    throw;
                }
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors("AllowReact");
            app.UseHttpsRedirection();

            //app.UseAuthorization();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
