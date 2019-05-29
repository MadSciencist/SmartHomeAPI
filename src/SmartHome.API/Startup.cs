using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet.Server;
using Newtonsoft.Json;
using SmartHome.API.Extensions;
using SmartHome.Core.DataAccess.InitialLoad;
using SmartHome.Core.Infrastructure.Validators;
using SmartHome.Core.IoC;
using SmartHome.Core.MqttBroker;
using SmartHome.Core.Services;
using System;
using System.Linq;
using System.Reflection;
using SmartHome.Core.Infrastructure;

namespace SmartHome.API
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }
        private readonly IConfiguration _configuration;

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

            // Create custom BadRequest response for built-in validator
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var result = new ServiceResult<object>
                    {
                        Alerts = context.ModelState.Select(x =>
                            new Alert(x.Value.Errors.FirstOrDefault()?.ErrorMessage, MessageType.Error)).ToList()
                    };

                    return new BadRequestObjectResult(result);
                };
            });

            // Security
            services.AddSqlIdentityPersistence(_configuration);
            services.AddJwtAuthentication(_configuration);
            services.AddAuthorizationPolicies();

            // JWT Token handling
            services.AddTokenBuilder();

            // Add AutoMapper configs
            services.AddAutoMapper(Assembly.GetAssembly(typeof(INodeService)));

            // CORS for dev env
            services.AddDefaultCorsPolicy();

            // Api docs gen
            services.AddConfiguredSwagger();

            // This allows access http context and user in constructor
            services.AddHttpContextAccessor();

            // Register SmartHome dependencies using Autofac container
            var builder = CoreDependencies.Register();
            builder.Populate(services);
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

            var mqttService = ApplicationContainer.Resolve<IMqttService>();
            mqttService.ServerOptions = mqttOptions;
            mqttService.StartBroker().Wait();
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
