using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SmartHome.API.Extensions;
using System;
using SmartHome.API.Persistence;

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
                .AddJsonOptions(json => json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            // Security
            services.AddSqlIdentityPersistence(_configuration);
            services.AddJwtAuthentication(_configuration);
            services.AddAuthorizationPolicies();

            // BL & Services
            services.RegisterAppServicesToIocContainer();

            // CORS for dev env
            services.AddDefaultCorsPolicy();

            // Api docs gen
            services.AddConfiguredSwagger();

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterAppServicesToAutofacContainer();
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseStatusCodePages();
                app.UseCors("CorsPolicy");               
            }

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
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                //var identityContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
                //identityContext.Database.Migrate();
                //identityContext.SaveChanges();

                IdentityInitialLoad.Seed(app.ApplicationServices).Wait();
                AppInitialLoad.Seed(app.ApplicationServices).Wait();
            }
        }
    }
}
