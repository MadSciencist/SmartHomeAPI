using Autofac;
using SmartHome.Core.BusinessLogic.TopicResolvers;
using System.Linq;
using System.Reflection;

namespace SmartHome.Core.Extensions
{
    public static class IoCExtensions
    {
        /// <summary>
        /// Register all the topic resolvers as named instanced into IoC container
        /// They can be resolved using fully qualified name
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterTopicResolvers(this ContainerBuilder builder)
        {
            var resolversAsm = Assembly.GetAssembly(typeof(ITopicResolver)).GetTypes();

            var resolvers = resolversAsm.ToDictionary(ex => ex.FullName);

            foreach (var resolver in resolvers)
            {
                if(resolver.Key.Contains("TopicResolvers"))
                builder.RegisterType(resolver.Value)
                    .Named<object>(resolver.Key);
            }
        }
    }
}
