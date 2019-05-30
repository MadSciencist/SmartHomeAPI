using Autofac;
using SmartHome.Core.Contracts.Mqtt.Control;
using SmartHome.Core.Contracts.Rest.Control;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.MqttBroker;
using SmartHome.Core.MqttBroker.MessageHandling;
using SmartHome.Core.Services;
using System.Linq;
using System.Reflection;
using SmartHome.Core.RestClient;

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

            Builder.RegisterType<MqttMessageProcessor>().InstancePerDependency();
            Builder.RegisterType<MessageInterceptor>().InstancePerDependency();

            Builder.RegisterType<NodeRepository>().As<INodeRepository>().InstancePerDependency();
            Builder.RegisterType<NodeDataRepository>().As<INodeDataRepository>().InstancePerDependency();
            Builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerDependency();

            Builder.RegisterType<NodeService>().As<INodeService>().InstancePerDependency();
            Builder.RegisterType<NodeDataService>().As<INodeDataService>().InstancePerDependency();
            Builder.RegisterType<DictionaryService>().As<IDictionaryService>().InstancePerDependency();
            Builder.RegisterType<ControlStrategyService>().As<IControlStrategyService>().InstancePerDependency();

            Builder.RegisterType<MqttService>().As<IMqttService>().SingleInstance();
            Builder.RegisterType<PersistentHttpClient>().SingleInstance();

            return Builder;
        }

        private static void AddContractsRestControlAssembly()
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

        private static void AddContractsMqttMessageHandlersAssembly()
        {
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
