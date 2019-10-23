using Autofac;
using Matty.Framework.Abstractions;
using Matty.Framework.Utils;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using SmartHome.Core.Control;
using SmartHome.Core.Data.Repository;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Entities.SchedulingEntity;
using SmartHome.Core.Infrastructure.AssemblyScanning;
using SmartHome.Core.MessageHanding;
using SmartHome.Core.MqttBroker;
using SmartHome.Core.MqttBroker.MessageHandling;
using SmartHome.Core.Repositories;
using SmartHome.Core.RestClient;
using SmartHome.Core.Scheduling;
using SmartHome.Core.Scheduling.Jobs;
using SmartHome.Core.Security;
using SmartHome.Core.Services;
using SmartHome.Core.Services.Abstractions;
using System;
using System.Collections.Generic;
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
            RegisterContractsDynamically();

            Builder.RegisterType<SyntheticDictionaryService>().InstancePerDependency();
            Builder.RegisterType<PhysicalPropertyService>().As<IPhysicalPropertyService>().InstancePerDependency();
            Builder.RegisterType<MessageInterceptor>().InstancePerDependency();
            Builder.RegisterType<NodeAuthorizationProvider>().As<IAuthorizationProvider<Node>>().InstancePerDependency();
            Builder.RegisterType<ScheduleAuthorizationProvider>().As<IAuthorizationProvider<ScheduleEntity>>().InstancePerDependency();
            Builder.RegisterType<MqttMessageProcessor>().As<IMessageProcessor<MqttMessageDto>>().InstancePerDependency();
            Builder.RegisterType<RestMessageProcessor>().As<IMessageProcessor<RestMessageDto>>().InstancePerDependency();

            Builder.RegisterType<NodeRepository>().As<INodeRepository>().InstancePerDependency();
            Builder.RegisterType<NodeDataRepository>().As<INodeDataRepository>().InstancePerDependency();
            Builder.RegisterType<StrategyRepository>().As<IControlStrategyRepository>().InstancePerDependency();
            Builder.RegisterType<AppUserNodeLinkRepository>().As<IAppUserNodeLinkRepository>().InstancePerDependency();
            Builder.RegisterType<DictionaryRepository>().As<IDictionaryRepository>().InstancePerDependency();
            Builder.RegisterType<SchedulesPersistenceRepository>().As<ISchedulesPersistenceRepository>().InstancePerDependency();
            Builder.RegisterType<PhysicalPropertyRepository>().As<IPhysicalPropertyRepository>().InstancePerDependency();
            Builder.RegisterGeneric(typeof(GenericRepository<,>)).As(typeof(ITransactionalRepository<,>)).InstancePerDependency();

            Builder.RegisterType<NodeService>().As<INodeService>().InstancePerDependency();
            Builder.RegisterType<NodeDataService>().As<INodeDataService>().InstancePerDependency();
            Builder.RegisterType<DictionaryService>().As<IDictionaryService>().InstancePerDependency();
            Builder.RegisterType<UiConfigurationService>().As<IUiConfigurationService>().InstancePerDependency();
            Builder.RegisterType<SchedulingService>().As<ISchedulingService>().InstancePerDependency();
            Builder.RegisterType<ControlStrategyService>().As<IControlStrategyService>().InstancePerDependency();

            Builder.RegisterType<MqttBroker.MqttBroker>().As<IMqttBroker>().SingleInstance();
            Builder.RegisterType<PersistentHttpClient>().SingleInstance();
            Builder.RegisterType<NotificationService>().SingleInstance();

            // Scheduler
            Builder.RegisterType<SchedulerHostedService>().As<IHostedService>().InstancePerDependency();

            Builder.RegisterType<JobFactory>().As<IJobFactory>().SingleInstance();
            Builder.RegisterType<StdSchedulerFactory>().As<ISchedulerFactory>().SingleInstance();
            // Jobs
            Builder.RegisterType<ExecuteNodeCommandJob>().SingleInstance();

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

                var commandExecutors = asmClasses.Where(x => typeof(IControlCommand).IsAssignableFrom(x.Value));
                RegisterNamed<IControlCommand>(commandExecutors);

                var messageHandlers = asmClasses.Where(x => AssemblyUtils.IsAssignableToGenericType(typeof(IMessageHandler<>), x.Value));
                RegisterNamed<object>(messageHandlers);

                var mappers = asmClasses.Where(x => typeof(INodeDataMapper).IsAssignableFrom(x.Value));
                RegisterNamed<INodeDataMapper>(mappers);
            }
        }

        private static void RegisterNamed<TResolvable>(IEnumerable<KeyValuePair<string, Type>> contractLibs)
        {
            foreach (var (key, value) in contractLibs)
            {
                Builder.RegisterType(value)
                    .Named<object>(key)
                    .As<TResolvable>()
                    .PropertiesAutowired();
            }
        }
    }
}
