using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MQTTnet.Server;
using Newtonsoft.Json;
using SmartHome.API.Extensions;
using SmartHome.API.Hubs;
using SmartHome.Core.DataAccess.InitialLoad;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Infrastructure.AssemblyScanning;
using SmartHome.Core.Infrastructure.Validators;
using SmartHome.Core.IoC;
using SmartHome.Core.MqttBroker;
using SmartHome.Core.Services;
using System;
using System.Linq;
using System.Reflection;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace SmartHome.API
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }
        public IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(json =>
                {
                    json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    json.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    json.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .AddFluentValidation(x =>
                {
                    x.RegisterValidatorsFromAssemblyContaining<NodeDtoValidator>();
                });

            // Create custom BadRequest response for built-in validator
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var result = new ServiceResult<object>
                    {
                        Alerts = context.ModelState
                        .Where(x => !string.IsNullOrEmpty(x.Value.Errors.FirstOrDefault()?.ErrorMessage))
                        .Select(x => new Alert(x.Value.Errors.FirstOrDefault()?.ErrorMessage, MessageType.Error)).ToList()
                    };

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

            services.AddSignalR(settings => { settings.EnableDetailedErrors = Environment.IsDevelopment(); });

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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMapper autoMapper,
            ILogger<Startup> logger)
        {
            ContractAssemblyAssertions.Logger = logger;
            ContractAssemblyAssertions.AssertValidConfig();
            autoMapper.ConfigurationProvider.AssertConfigurationIsValid();

            InitializeDatabase(app);

            var mqttOptions = new MqttServerOptionsBuilder()
                .WithDefaultEndpointPort(Configuration.GetValue<int>("MqttBroker:Port"))
                .WithConnectionBacklog(Configuration.GetValue<int>("MqttBroker:MaxBacklog"))
                .WithClientId(Configuration.GetValue<string>("MqttBroker:ClientId"))
                .Build();

            // Create singleton instance of mqtt broker
            var mqttService = ApplicationContainer.Resolve<IMqttBroker>();
            mqttService.ServerOptions = mqttOptions;
            mqttService.StartAsync().Wait();

            /* App pipeline */
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseStatusCodePages();
            }

            app.UseCors("CorsPolicy");

            // serve statics
            app.UseDefaultFiles(); // will run index.html by default
            app.UseStaticFiles(); // will serve wwwroot by default

            // auth middleware
            app.UseAuthentication();

            // custom logging middleware 
            app.UseLoggingExceptionHandler();

            // standard MVC middleware
            app.UseMvc();

            app.UseSignalR(routes =>
            {
                var endpoint = Configuration["NotificationEndpoint"];
                routes.MapHub<NotificationHub>(endpoint);
            });

            // docs gen and UI
            app.UseSwagger();
            app.UseSwaggerUI(s => { s.SwaggerEndpoint("/swagger/dev/swagger.json", "v1"); });

            var useHealthChecks = Configuration.GetValue<bool>("HealthChecks:Enable");
            if (useHealthChecks)
            { 
                app.UseHealthChecks(Configuration["HealthChecks:Endpoint"], new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                    AllowCachingResponses = true
                });

                app.UseHealthChecksUI(options =>
                {
                    options.ApiPath = "/api/health-ui-api";
                    options.ResourcesPath = "/api/ui/resources";
                    options.UseRelativeResourcesPath = false;
                    options.UseRelativeApiPath = false;
                    options.UIPath = Configuration["HealthChecks:UiEndpoint"];
                });
            }
        }

        private static void InitializeDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var initialLoadFacade = new InitialLoadFacade(scope.ServiceProvider);
                initialLoadFacade.Seed().Wait();
            }
        }
    }
}
