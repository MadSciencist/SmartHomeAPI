using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet.Server;
using Newtonsoft.Json;
using SmartHome.API.Extensions;
using SmartHome.API.Security.Token;
using SmartHome.Core.Contracts.Mqtt.Control.Extensions;
using SmartHome.Core.Contracts.Rest.Control.Extensions;
using SmartHome.Core.DataAccess.InitialLoad;
using SmartHome.Core.MqttBroker;
using System;
using System.Reflection;
using AutoMapper;
using FluentValidation.AspNetCore;
using SmartHome.Core.Contracts.Mqtt.MessageHandling.Extensions;
using SmartHome.Core.Infrastructure.Validators;
using SmartHome.Core.Services;

namespace SmartHome.API
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }
        private readonly IConfiguration _configuration;
        private IMqttService _mqttService;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(json => json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<NodeDtoValidator>());

            // remove default ASP.NET Core validator - We are going to use FluentValidation
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // Security
            services.AddSqlIdentityPersistence(_configuration);
            services.AddJwtAuthentication(_configuration);
            services.AddAuthorizationPolicies();

            // BL & Services
            services.RegisterAppServicesToIocContainer();

            // Add AutoMapper configs
            services.AddAutoMapper(Assembly.GetAssembly(typeof(INodeService)));

            // CORS for dev env
            services.AddDefaultCorsPolicy();

            // Api docs gen
            services.AddConfiguredSwagger();

            // This allows access http context and user in constructor
            services.AddHttpContextAccessor();

            var builder = new ContainerBuilder();
            builder.Populate(services);

            // Add REST and MQTT contracts
            builder.RegisterRestNodeContracts();
            builder.RegisterMqttNodeContracts();

            builder.RegisterMqttMessageHandlers();

            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration conf, IMapper autoMapper)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseStatusCodePages();
                app.UseCors("CorsPolicy");               
            }

            autoMapper.ConfigurationProvider.AssertConfigurationIsValid();

            // custom logging middleware 
            app.UseLoggingExceptionHandler();

            // serve statics
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // auth middleware
            app.UseAuthentication();

            // standard MVC middleware
            app.UseMvc();

            // docs gen and UI
            app.UseSwagger();
            app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/dev/swagger.json", "Home Sensor Server API"));
            
            InitializeDatabase(app);

            var mqttOptions = new MqttServerOptionsBuilder()
                .WithDefaultEndpointPort(conf.GetValue<int>("MqttBroker:Port"))
                .WithConnectionBacklog(conf.GetValue<int>("MqttBroker:MaxBacklog"))
                .WithClientId(conf.GetValue<string>("MqttBroker:ClientId"))
                .Build();

            _mqttService = ApplicationContainer.Resolve<IMqttService>();
            _mqttService.ServerOptions = mqttOptions;
            _mqttService.StartBroker().Wait();
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
