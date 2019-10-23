using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Matty.Framework;
using Matty.Framework.Enums;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet.Server;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SmartHome.API.Extensions;
using SmartHome.API.Hubs;
using SmartHome.Core.Data.InitialLoad;
using SmartHome.Core.Infrastructure.AssemblyScanning;
using SmartHome.Core.Infrastructure.Validators;
using SmartHome.Core.IoC;
using SmartHome.Core.MqttBroker;
using SmartHome.Core.Services.Abstractions;
using System;
using System.Linq;
using System.Reflection;

namespace SmartHome.API
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssemblyContaining<NodeDtoValidator>();
                });

            // Create custom BadRequest response for built-in validator
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var result = new ServiceResult<object>(context.ModelState
                        .Where(x => !string.IsNullOrEmpty(x.Value.Errors.FirstOrDefault()?.ErrorMessage))
                        .Select(x => new Alert(x.Value.Errors.FirstOrDefault()?.ErrorMessage, MessageType.Error))
                        .ToList());

                    return new BadRequestObjectResult(result);
                };
            });

            // Security
            services.AddSqlIdentityPersistence(Configuration, Environment);
            services.AddJwtAuthentication(Configuration);
            services.AddAuthorizationPolicies();

            // Basic caching
            services.AddMemoryCache();

            // JWT Token handling
            services.AddApiServices();

            // Add AutoMapper configs
            services.AddAutoMapper(Assembly.GetAssembly(typeof(INodeService)));

            // CORS for dev env
            services.AddDefaultCorsPolicy(Environment);

            // Api docs gen
            services.AddConfiguredSwagger();

            // This allows access http context and user in constructor
            services.AddHttpContextAccessor();

            services.AddSignalR(settings => { settings.EnableDetailedErrors = Environment.IsDevelopment(); })
                .AddNewtonsoftJsonProtocol();

            var useHealthChecks = Configuration.GetValue<bool>("HealthChecks:Enable");
            if (useHealthChecks)
            {
                var hubUri = $"{Configuration[WebHostDefaults.ServerUrlsKey]}{Configuration["NotificationEndpoint"]}";
                services.AddHealthChecks()
                    .AddMySql(Configuration["ConnectionStrings:MySql"])
                    .AddSignalRHub(hubUri)
                    .AddCheck<MqttBrokerHealthCheck>("mqtt_broker");
                services.AddHealthChecksUI();
            }

            services.AddMediatR(Assembly.GetAssembly(typeof(Startup)), Assembly.GetAssembly(typeof(INodeService)));

            // Register SmartHome dependencies using Autofac container
            var builder = CoreDependencies.Register();
            builder.Populate(services);
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper autoMapper,
            ILogger<Startup> logger)
        {
            ContractAssemblyAssertions.Logger = logger;
            ContractAssemblyAssertions.AssertValidConfig();
            autoMapper.ConfigurationProvider.AssertConfigurationIsValid();

            InitializeDatabase(app, logger);

            var mqttOptions = new MqttServerOptionsBuilder()
                .WithDefaultEndpointPort(Configuration.GetValue<int>("MqttBroker:Port"))
                .WithConnectionBacklog(Configuration.GetValue<int>("MqttBroker:MaxBacklog"))
                .WithClientId(Configuration.GetValue<string>("MqttBroker:ClientId"))
                .Build();

            // Create singleton instance of mqtt broker
            var mqttService = ApplicationContainer.Resolve<IMqttBroker>();
            mqttService.ServerOptions = mqttOptions;
            mqttService.StartAsync().Wait();

            var signalREndpoint = Configuration["NotificationEndpoint"];
            var healthCheckEndpoint = Configuration["HealthChecks:Endpoint"];
            var useHealthChecks = Configuration.GetValue<bool>("HealthChecks:Enable");

            /* App pipeline */
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }

            if (useHealthChecks)
            {
                app.UseHealthChecksUI(options =>
                {
                    options.ApiPath = "/api/health-ui-api";
                    options.ResourcesPath = "/api/ui/resources";
                    options.UseRelativeResourcesPath = false;
                    options.UseRelativeApiPath = false;
                    options.UIPath = Configuration["HealthChecks:UiEndpoint"];
                });
            }

            /* MVC Pipeline */
            // serve statics - must be called before UseRouting
            app.UseDefaultFiles(); // will run index.html by default
            app.UseStaticFiles(); // will serve wwwroot by default

            app.UseRouting();

            // must be called after UseRouting
            app.UseCors("CorsPolicy");

            // auth middleware - must be after UseRouting and UseCors but before UseEndpoints
            app.UseAuthentication();
            app.UseAuthorization();

            // custom logging middleware 
            app.UseLoggingExceptionHandler();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>(signalREndpoint);

                if (useHealthChecks)
                {
                    endpoints.MapHealthChecks(healthCheckEndpoint, new HealthCheckOptions
                    {
                        Predicate = _ => true,
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                        AllowCachingResponses = true
                    });
                }
            });

            // Open-API doc gen
            app.UseSwagger();
            app.UseSwaggerUI(s => { s.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"); });
        }

        private static void InitializeDatabase(IApplicationBuilder app, ILogger logger)
        {
            try
            {
                using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
                var initialLoadFacade = new InitialLoadFacade(scope.ServiceProvider);
                initialLoadFacade.Seed().Wait();
            }
            catch (Exception ex)
            when (ex is AggregateException
                    && ex.InnerException is InvalidOperationException iox
                    && iox.InnerException is MySqlException mysqlEx)
            {
                logger.LogError("Cannot seed database, MySQL-level exception. DB server might be off.", mysqlEx);
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError("Cannot seed database, DB server might be off.", ex);
                throw;
            }
        }
    }
}
