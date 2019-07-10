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
using SmartHome.API.Hubs;
using SmartHome.Core.DataAccess.InitialLoad;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Infrastructure.Validators;
using SmartHome.Core.IoC;
using SmartHome.Core.MqttBroker;
using SmartHome.Core.Services;
using System;
using System.Linq;
using System.Reflection;

namespace SmartHome.API
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }
        public IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get;  }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(json => {
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
                        Alerts = context.ModelState.Select(x =>
                            new Alert(x.Value.Errors.FirstOrDefault()?.ErrorMessage, MessageType.Error)).ToList()
                    };

                    return new BadRequestObjectResult(result);
                };
            });

            // Security
            services.AddSqlIdentityPersistence(Configuration, Environment);
            services.AddJwtAuthentication(Configuration);
            services.AddAuthorizationPolicies();

            // JWT Token handling
            services.AddApiServices();

            // Add AutoMapper configs
            services.AddAutoMapper(Assembly.GetAssembly(typeof(INodeService))); // ToDo move to IoC project

            // CORS for dev env
            services.AddDefaultCorsPolicy(Environment);

            // Api docs gen
            services.AddConfiguredSwagger();
            
            // This allows access http context and user in constructor
            services.AddHttpContextAccessor();

            services.AddSignalR(settings => { settings.EnableDetailedErrors = true; });

            services.AddHostedService<MqttService>();

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

            app.UseSignalR(routes =>
            {
                var endpoint = conf.GetValue<string>("NotificationEndpoint");
                routes.MapHub<NotificationHub>(endpoint);
            });

            // docs gen and UI
            app.UseSwagger();
            app.UseSwaggerUI(s => { s.SwaggerEndpoint("/swagger/dev/swagger.json", "v1"); });

            InitializeDatabase(app);

            // Create singleton instance of notifier
            var hubNotifier = ApplicationContainer.Resolve<HubNotifier>();
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
