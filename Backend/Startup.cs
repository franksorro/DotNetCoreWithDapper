using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.S3;
using AutoMapper;
using Backend.Interfaces;
using Backend.Repositories;
using Backend.Services;
using Core.Middlewares;
using FS.Repositories;
using FS.Services;
using FS.Interfaces;
using Enyim.Caching.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Data;
using MySql.Data.MySqlClient;
using System;

namespace Backend
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IDbConnection dbConnection;

        private readonly string swaggerTitle = "Starting .NET Core with Dapper project";

        private readonly string apiKeyName;

        private readonly IEnumerable<Server> memCachedServers;

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

            apiKeyName = configuration.GetValue<string>("Authorization:ApiKeyName");

            memCachedServers = configuration.GetSection("MemCached:Servers").Get<IEnumerable<Server>>();

            dbConnection = new MySqlConnection(configuration.GetConnectionString("DataSource"));
            if (dbConnection == null)
                throw new ArgumentNullException("DataSource connection cannot be null");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(opts =>
            {
                opts.ResourcesPath = "Resources";
            });

            services.AddMvc().AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(opts =>
            {
                List<CultureInfo> supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("it-IT")
                };

                opts.DefaultRequestCulture = new RequestCulture(culture: supportedCultures.First(), uiCulture: supportedCultures.First());
                opts.SupportedCultures = supportedCultures;
                opts.SupportedUICultures = supportedCultures;
            });

            services
                .Configure<AppSettings>(configuration)
                .AddControllers()
            ;

            services.AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(1, 0);
                setup.AssumeDefaultVersionWhenUnspecified = true;
                setup.ReportApiVersions = true;
            });

            services
                .AddSingleton<IMapper>(new Mapper(AutoMapperSetup.SetupMapping()))
                .AddSingleton<IDapperService>(new DapperService(dbConnection))
                .AddSingleton<IMemCachedService, MemCachedService>()
                .AddSingleton<IClientCacheRepository, ClientCacheRepository>()
                .AddSingleton<IClientCacheService, ClientCacheService>()
                .AddSingleton<AuthFilter>()
            ;

            services
                .AddSingleton<ITestRepository, TestRepository>()
                .AddSingleton<TestService>()
            ;

            services
                .AddDistributedMemoryCache()
                .AddEnyimMemcached(memcachedClientOptions => {
                    memcachedClientOptions.Servers = memCachedServers.ToList();
                });

            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc(
                    name: "v1", new OpenApiInfo
                    {
                        Title = swaggerTitle,
                        Version = "v2",
                        Contact = new OpenApiContact
                        {
                            Name = "FRΛƝCƎƧCØ ƧØЯRƎƝTIƝØ",
                            Email = "franksorro@gmail.com",
                            Url = new System.Uri("https://github.com/franksorro")
                        }
                    });

                opts.SwaggerDoc(
                    name: "v2", new OpenApiInfo
                    {
                        Title = swaggerTitle,
                        Version = "v2",
                        Contact = new OpenApiContact
                        {
                            Name = "FRΛƝCƎƧCØ ƧØЯRƎƝTIƝØ",
                            Email = "franksorro@gmail.com",
                            Url = new System.Uri("https://github.com/franksorro")
                        }
                    });

                opts.AddSecurityDefinition(apiKeyName, new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Name = apiKeyName,
                    In = ParameterLocation.Header
                });

                opts.AddSecurityRequirement(new OpenApiSecurityRequirement()
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

            //---AWS setup---
            var awsOptions = configuration.GetAWSOptions();
            awsOptions.Credentials = new BasicAWSCredentials(
                configuration.GetValue<string>("AWS:AccessKeyId"),
                configuration.GetValue<string>("AWS:AWSSecretKey")
            );

            services
                .AddAWSService<IAmazonS3>(awsOptions)
                .AddAWSService<IAmazonDynamoDB>(awsOptions)
                .AddSingleton<IS3Service, S3Service>()
                .AddSingleton<IDynamoDBService, DynamoDBService>()
            ;
            //---*---
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

            IOptions<RequestLocalizationOptions> localizeOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizeOptions.Value);

            app
                .UseSwagger()
                .UseSwaggerUI(opts =>
                {
                    opts.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Services collection version 1");
                    opts.SwaggerEndpoint(url: "/swagger/v2/swagger.json", name: "Services collection version 2");
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
