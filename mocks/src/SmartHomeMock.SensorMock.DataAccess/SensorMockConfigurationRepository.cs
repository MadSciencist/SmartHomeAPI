using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SmartHomeMock.SensorMock.DataAccess.Interfaces;
using SmartHomeMock.SensorMock.Entities;
using SmartHomeMock.SensorMock.Entities.Configuration;

namespace SmartHomeMock.SensorMock.DataAccess
{
    public class SensorMockConfigurationRepository : ISensorMockConfigurationRepository
    {
        private const string ConfigurationFilePathKey = "SensorMockConfigurationPath";

        private readonly IConfiguration _configuration;

        public SensorMockConfigurationRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SensorMockConfiguration GetConfigurationById(int id)
        {
            var configText = File.ReadAllText(_configuration[ConfigurationFilePathKey]);

            var jsonConfigText = JObject.Parse(configText);

            var jsonConfig = jsonConfigText.SelectToken($"$.configurations[?(@.id == {id})]");

            return jsonConfig.ToObject<SensorMockConfiguration>();
        }
    }
}