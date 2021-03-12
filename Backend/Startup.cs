using System.Collections.Generic;
using AutoMapper;
using Backend.Interfaces;
using Backend.Middlewares;
using Backend.Repositories;
using Backend.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Backend
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration configuration;

        private readonly string swaggerName;
        private readonly string swaggerTitle;
        private readonly string swaggerVersion;
        private readonly string swaggerEndpoint;

        private readonly string apiKeyName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables()
            ;

            if (env.IsDevelopment())
                builder.AddUserSecrets<Program>();

            configuration = builder.Build();

            swaggerName = configuration.GetValue<string>("Swagger:Name");
            swaggerTitle = configuration.GetValue<string>("Swagger:Title");
            swaggerVersion = configuration.GetValue<string>("Swagger:Version");
            swaggerEndpoint = configuration.GetValue<string>("Swagger:Endpoint");

            apiKeyName = configuration.GetValue<string>("Authorization:ApiKeyName");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .Configure<AppSettings>(configuration)
                .AddControllers()
            ;

            services
                .AddSingleton<IMapper>(new Mapper(AutoMapperSetup.SetupMapping()))
                .AddSingleton<IDapperCore>(new DapperCore(configuration.GetConnectionString("DataSource")))
                .AddSingleton<IDapperAsyncService, DapperAsyncService>()
                .AddSingleton<ITestRepository, TestRepository>()
                .AddSingleton<TestService>()
            ;

            services.AddScoped<AuthFilter>();

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc(
                    name: swaggerName, new OpenApiInfo
                    {
                        Title = swaggerTitle,
                        Version = swaggerVersion
                    });

                opt.AddSecurityDefinition(apiKeyName, new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Name = apiKeyName,
                    In = ParameterLocation.Header
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = apiKeyName
                            }
                        },
                        new List<string>()
                    }
                });
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app
                .UseSwagger()
                .UseSwaggerUI(opts =>
                {
                    opts.SwaggerEndpoint(url: swaggerEndpoint, name: swaggerName);
                    opts.DocumentTitle = swaggerTitle;
                    opts.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
                });

            app
                .UseHttpsRedirection()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapControllerRoute(
                            name: "api",
                            pattern: "api/{controller}/{id?}");
                    endpoints.MapControllerRoute(
                            name: "default",
                            pattern: "{controller=Home}/{action=Index}/{id?}");
                })
            ;
        }
    }
}
