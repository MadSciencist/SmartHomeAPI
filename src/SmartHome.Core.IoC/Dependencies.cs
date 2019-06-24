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
using SmartHome.Core.Authorization;
using SmartHome.Core.Domain.Notification;
using SmartHome.Core.Infrastructure.SyntheticDictionaries;

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

            Builder.RegisterType<SyntheticDictionaryService>().InstancePerDependency();
            Builder.RegisterType<MqttMessageProcessor>().InstancePerDependency();
            Builder.RegisterType<MessageInterceptor>().InstancePerDependency();
            Builder.RegisterType<NodeAuthorizationProvider>().InstancePerDependency();

            Builder.RegisterType<NodeRepository>().As<INodeRepository>().InstancePerDependency();
            Builder.RegisterType<NodeDataRepository>().As<INodeDataRepository>().InstancePerDependency();
            Builder.RegisterType<StrategyRepository>().As<IStrategyRepository>().InstancePerDependency();
            Builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerDependency();

            Builder.RegisterType<NodeService>().As<INodeService>().InstancePerDependency();
            Builder.RegisterType<NodeDataService>().As<INodeDataService>().InstancePerDependency();
            Builder.RegisterType<DictionaryService>().As<IDictionaryService>().InstancePerDependency();
            Builder.RegisterType<ControlStrategyService>().As<IControlStrategyService>().InstancePerDependency();

            Builder.RegisterType<MqttService>().As<IMqttService>().SingleInstance();
            Builder.RegisterType<PersistentHttpClient>().SingleInstance();
            Builder.RegisterType<NotificationQueue>().SingleInstance();
            Builder.RegisterType<NotificationService>().SingleInstance();

            return Builder;
        }

        private static void AddContractsRestControlAssembly()
        {
            var contractsAsm = Assembly.GetAssembly(typeof(IRestControlStrategy)).GetTypes();
            var contracts = contractsAsm.Where(x=> x.IsClass && !x.IsInterface && !x.IsAbstract && typeof(IRestControlStrategy).IsAssignableFrom(x))
                .ToDictionary(ex => ex.FullName);

            foreach (var contract in contracts)
            {
                Builder.RegisterType(contract.Value)
                    .Named<object>(contract.Key);
            }
        }

        private static void AddContractsMqttControlAssembly()
        {
            var contractsAsm = Assembly.GetAssembly(typeof(IMqttControlStrategy)).GetTypes();
            var contracts = contractsAsm.Where(x => x.IsClass && !x.IsInterface && !x.IsAbstract && typeof(IMqttControlStrategy).IsAssignableFrom(x))
                .ToDictionary(ex => ex.FullName);

            foreach (var contract in contracts)
            {
                Builder.RegisterType(contract.Value)
                    .Named<object>(contract.Key);
            }
        }

        private static void AddContractsMqttMessageHandlersAssembly()
        {
            var handlersAsm = Assembly.Load("SmartHome.Core.Contracts.Mqtt.MessageHandling").GetTypes();
            var resolvers = handlersAsm.Where(x => x.IsClass && !x.IsAbstract && !x.IsInterface)
                .ToDictionary(ex => ex.FullName);

            foreach (var resolver in resolvers)
            {
                if (resolver.Key.Contains("MessageHandling"))
                    Builder.RegisterType(resolver.Value)
                        .Named<object>(resolver.Key);
            }
        }
    }
}
