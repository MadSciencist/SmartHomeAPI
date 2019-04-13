using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SmartHome.API.Extensions;
using SmartHome.API.Persistence.App;
using SmartHome.API.Persistence.Identity;
using System;

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
            services.AddSqlPersistence(_configuration);

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
                app.UseCors("CorsPolicy");               
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseStatusCodePages();
            app.UseAuthentication();
            app.UseMvc();
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
                NodeTypeInitialLoad.Seed(app.ApplicationServices).Wait();
            }
        }
    }
}
