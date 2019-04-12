using Microsoft.Extensions.DependencyInjection;
using SmartHome.API.Services.Crud;

namespace SmartHome.API.Services.Extensions
{
    public static class ServicesExtensions
    {
        public static void RegisterServicesToIocContainer(this IServiceCollection services)
        {
            services.AddTransient<ICrudNodeService, CrudNodeService>();
        }
    }
}