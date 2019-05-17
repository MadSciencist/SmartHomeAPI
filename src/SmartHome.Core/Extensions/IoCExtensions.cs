using Autofac;
using System.Linq;
using System.Reflection;
using SmartHome.Core.BusinessLogic.MqttMessageResolvers;

namespace SmartHome.Core.Extensions
{
    public static class IoCExtensions
    {
        /// <summary>
        /// Register all the mqtt message resolvers as named instanced into IoC container
        /// They can be resolved using fully qualified name
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterTopicResolvers(this ContainerBuilder builder)
        {
            var resolversAsm = Assembly.GetAssembly(typeof(IMqttMessageResolver)).GetTypes();

            var resolvers = resolversAsm.ToDictionary(ex => ex.FullName);

            foreach (var resolver in resolvers)
            {
                if(resolver.Key.Contains("MqttMessageResolvers"))
                builder.RegisterType(resolver.Value)
                    .Named<object>(resolver.Key);
            }
        }
    }
}
