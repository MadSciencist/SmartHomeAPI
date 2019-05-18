using Autofac;
using SmartHome.Core.MessageHandlers;
using System.Linq;
using System.Reflection;

namespace SmartHome.Core.Contracts.Mqtt.MessageHandling.Extensions
{
    public static class IoCExtensions
    {
        /// <summary>
        /// Register all the mqtt message resolvers as named instanced into IoC container
        /// They can be resolved using fully qualified name
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterMqttMessageHandlers(this ContainerBuilder builder)
        {
            var resolversAsm = Assembly.GetExecutingAssembly().GetTypes();

            var resolvers = resolversAsm.ToDictionary(ex => ex.FullName);

            foreach (var resolver in resolvers)
            {
                if(resolver.Key.Contains("MessageHandling"))
                builder.RegisterType(resolver.Value)
                    .Named<object>(resolver.Key);
            }
        }
    }
}
