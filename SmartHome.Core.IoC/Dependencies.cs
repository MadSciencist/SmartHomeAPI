using Autofac;
using SmartHome.Core.Contracts.Mqtt.Control;
using SmartHome.Core.Contracts.Rest.Control;
using System.Linq;
using System.Reflection;

namespace SmartHome.Core.IoC
{
    public static class CoreDependencies
    {
        private static readonly ContainerBuilder Builder;

        static CoreDependencies()
        {
            Builder = new ContainerBuilder();
        }

        public static ContainerBuilder Register()
        {
            AddContractsMqttControlAssembly();
            AddContractsRestControlAssembly();
            AddContractsMqttMessageHandlersAssembly();

            return Builder;
        }

        public static void AddContractsRestControlAssembly()
        {
            var commandExecutorAsm = Assembly.GetAssembly(typeof(IRestControlStrategy)).GetTypes();

            var contracts = commandExecutorAsm.ToDictionary(ex => ex.FullName);

            foreach (var contract in contracts)
            {
                Builder.RegisterType(contract.Value)
                    .Named<object>(contract.Key);
            }
        }

        private static void AddContractsMqttControlAssembly()
        {
            var commandExecutorAsm = Assembly.GetAssembly(typeof(IMqttControlStrategy)).GetTypes();

            var contracts = commandExecutorAsm.ToDictionary(ex => ex.FullName);

            foreach (var contract in contracts)
            {
                Builder.RegisterType(contract.Value)
                    .Named<object>(contract.Key);
            }
        }

        public static void AddContractsMqttMessageHandlersAssembly()
        {
            //var resolversAsm = Assembly.GetAssembly(typeof(IMqttControlStrategy)).GetTypes();
            var resolversAsm = Assembly.Load("SmartHome.Core.Contracts.Mqtt.MessageHandling").GetTypes();

            var resolvers = resolversAsm.ToDictionary(ex => ex.FullName);

            foreach (var resolver in resolvers)
            {
                if (resolver.Key.Contains("MessageHandling"))
                    Builder.RegisterType(resolver.Value)
                        .Named<object>(resolver.Key);
            }
        }
    }
}
