using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SmartHome.Repositories.Extensions;
using SmartHome.Security.Extensions;
using SmartHome.Security.Persistence;
using Swashbuckle.AspNetCore.Swagger;

namespace SmartHome.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(json => json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            // Security
            services.RegisterIdentityToIocContainer();
            services.AddSqlIdentityPersistence(_configuration);
            services.AddJwtAuthentication(_configuration);
            services.AddAuthorizationPolicies();

            // DAL
            services.RegisterRepositoriesToIocContainer();

            // CORS for dev env
            services.AddDefaultCorsPolicy();

            // Api docs gen
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("dev", new Info { Title = "Home Sensor Server API", Version = "v1", Contact = new Contact { Email = "mkrysz1337@gmail.com" } });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("CorsPolicy");
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseStatusCodePages();
            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(swagger =>
            {
                swagger.SwaggerEndpoint("/swagger/dev/swagger.json", "Home Sensor Server API");
            });

            AppIdentityDbContext.SeedRolesAndUser(app.ApplicationServices).Wait();
        }
    }

    public static class CorsExtensions
    {
        public static IServiceCollection AddDefaultCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(settings =>
            {
                settings.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            return services;
        }
    }
}
