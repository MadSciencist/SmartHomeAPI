using Autofac;
using SmartHome.Core.Control;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Domain.Notification;
using SmartHome.Core.Infrastructure.AssemblyScanning;
using SmartHome.Core.Infrastructure.SyntheticDictionaries;
using SmartHome.Core.MqttBroker;
using SmartHome.Core.MqttBroker.MessageHandling;
using SmartHome.Core.RestClient;
using SmartHome.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SmartHome.Core.Dto;
using SmartHome.Core.MessageHanding;
using SmartHome.Core.Security;

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
            RegisterContractsDynamically();

            Builder.RegisterType<SyntheticDictionaryService>().InstancePerDependency();
            Builder.RegisterType<MessageInterceptor>().InstancePerDependency();
            Builder.RegisterType<NodeAuthorizationProvider>().InstancePerDependency();
            Builder.RegisterType<MqttMessageProcessor>().As<IMessageProcessor<MqttMessageDto>>().InstancePerDependency();

            Builder.RegisterType<NodeRepository>().As<INodeRepository>().InstancePerDependency();
            Builder.RegisterType<NodeDataRepository>().As<INodeDataRepository>().InstancePerDependency();
            Builder.RegisterType<StrategyRepository>().As<IStrategyRepository>().InstancePerDependency();
            Builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerDependency();
            Builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerDependency();

            Builder.RegisterType<NodeService>().As<INodeService>().InstancePerDependency();
            Builder.RegisterType<NodeDataService>().As<INodeDataService>().InstancePerDependency();
            Builder.RegisterType<DictionaryService>().As<IDictionaryService>().InstancePerDependency();

            Builder.RegisterType<MqttBroker.MqttBroker>().As<IMqttBroker>().SingleInstance();
            Builder.RegisterType<PersistentHttpClient>().SingleInstance();
            Builder.RegisterType<NotificationQueue>().SingleInstance();
            Builder.RegisterType<NotificationService>().SingleInstance();

            return Builder;
        }

        private static void RegisterContractsDynamically()
        {
            foreach (var path in AssemblyScanner.GetContractsLibsPaths())
            {
                var asm = Assembly.LoadFile(path);
                var asmClasses = asm.GetTypes()
                    .Where(x => x.IsClass && !x.IsAbstract && !x.IsInterface)
                    .ToDictionary(x => x.FullName);

                var commandExecutors = asmClasses.Where(x => typeof(IControlStrategy).IsAssignableFrom(x.Value));
                RegisterNamed(commandExecutors);

                var messageHandlers = asmClasses.Where(x => AssemblyUtils.IsAssignableToGenericType(typeof(IMessageHandler<>), x.Value));
                RegisterNamed(messageHandlers);

                var mappers = asmClasses.Where(x => typeof(INodeDataMapper).IsAssignableFrom(x.Value));
                RegisterNamed(mappers);
            }
        }

        private static void RegisterNamed(IEnumerable<KeyValuePair<string, Type>> commandExecutors)
        {
            foreach (var (key, value) in commandExecutors)
            {
                Builder.RegisterType(value)
                    .Named<object>(key);
            }
        }
    }
}
