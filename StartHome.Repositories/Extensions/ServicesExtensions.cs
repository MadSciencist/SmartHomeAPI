using Microsoft.Extensions.DependencyInjection;

namespace SmartHome.Repositories.Extensions
{
    public static class ServicesExtensions
    {
        public static void RegisterRepositoriesToIocContainer(this IServiceCollection services)
        {
            services.AddTransient<INodeRepository, NodeRepository>();
        }
    }
}