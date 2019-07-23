using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using SmartHomeMock.SensorMock.DataAccess;
using SmartHomeMock.SensorMock.DataAccess.Interfaces;
using SmartHomeMock.SensorMock.Domain.Interfaces;

namespace SmartHomeMock.SensorMock.App
{
    class Program
    {
        private static IConfiguration _configuration;
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            Console.WriteLine("Pre Config.");

            SetUp();

            Console.WriteLine($"Post Config {_configuration["SensorMockConfigurationPath"]}");

            var configurationId = args.Length > 0
                ? int.TryParse(args[0], out var intConfId)
                    ? intConfId
                    : 1
                : 1;

            var mock = _serviceProvider.GetService<ISmartHomeMock>();

            mock.Run(configurationId);
        }

        private static void SetUp()
        {
            SetUpConfiguration();
            SetUpServices();
        }

        private static void SetUpConfiguration()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
        }

        private static void SetUpServices()
        {
            var collection = new ServiceCollection();
            collection.AddSingleton<IConfiguration>(_configuration);

            collection.AddTransient<ISmartHomeMock, Domain.SmartHomeMock>();
            collection.AddTransient<ISensorMockConfigurationRepository, SensorMockConfigurationRepository>();
            collection.AddTransient<IMqttFactory, MqttFactory>();
            collection.AddTransient<IMqttFactory, MqttFactory>();

            _serviceProvider = collection.BuildServiceProvider();
        }
    }
}
