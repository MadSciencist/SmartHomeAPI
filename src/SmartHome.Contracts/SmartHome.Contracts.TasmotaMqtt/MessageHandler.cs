using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartHome.Contracts.TasmotaMqtt.Domain.Models;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.ContractParams;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.MessageHanding;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SmartHome.Contracts.TasmotaMqtt
{
    public class Handler : MessageHandlerBase<MqttMessageDto>
    {
        private const string TasmotaLightKey = "light";
        private const string TasmotaRelay0Key = "POWER";
        private const string TasmotaDimmerKey = "Dimmer";
        private const string TasmotaColorTemperatureKey = "CT";
        private const string TasmotaEnergyStatusKey = "StatusSNS";

        public Handler(ILifetimeScope container, Node node) : base(container, node)
        {
        }

        public override async Task Handle(MqttMessageDto message)
        {
            try
            {
                var payload = JObject.Parse(message.Payload);

                // Due to tasmota complex responses, we need to check for complex types 'manually'
                if (await SaveLightComplexParam(payload)) return;
                if (await SavePowerConsumptionParam(payload)) return;
                if (await SaveRelayParam(payload)) return;
            }
            catch (JsonReaderException)
            {
                // Tasmota returns confirmation twice: once as json, second time normally
                // We just want to swallow the second, redundant message exception
            }
        }

        /// <summary>
        /// Check if the payload contains relay key.
        /// If so, then map it to system property and check if user wants to store this key data.
        /// Finally, save it.
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        private async Task<bool> SaveRelayParam(JObject payload)
        {
            if (!payload.ContainsKey(TasmotaRelay0Key)) return false;

            var mappedProperty = DataMapper.GetMapping(TasmotaRelay0Key);
            if (!Node.ShouldMagnitudeBeStored(mappedProperty)) return false;

            var sensorValue = payload.GetValue(TasmotaRelay0Key).ToString();
            await ProcessNodeMagnitude(mappedProperty, sensorValue);
            return true;
        }

        /// <summary>
        /// Check if the payload contains light key.
        /// If so, then map it to system property and check if user wants to store this key data.
        /// Finally, save it.
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        private async Task<bool> SaveLightComplexParam(JObject payload)
        {
            if (!payload.ContainsKey(TasmotaLightKey) ||
                !payload.ContainsKey(TasmotaDimmerKey) ||
                !payload.ContainsKey(TasmotaColorTemperatureKey))
                return false;

            var mappedMagnitudeKey = DataMapper.GetMapping(TasmotaLightKey);
            if (!Node.ShouldMagnitudeBeStored(mappedMagnitudeKey)) return false;

            var param = new LightParam
            {
                State = int.Parse(ApplyConversion("relay0", payload.GetValue(TasmotaRelay0Key).ToString())), // TODO complex physical properties
                Brightness = int.Parse(payload.GetValue(TasmotaDimmerKey).ToString()),
                LightTemperature = int.Parse(payload.GetValue("CT").ToString())
            };

            var value = JsonConvert.SerializeObject(param);
            await ProcessNodeMagnitude(mappedMagnitudeKey, value);
            return true;
        }

        /// <summary>
        /// Check if the payload contains power consumption key. If so, then map it to system property and check if user wants to store this key data.
        /// Finally, save it.
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        private async Task<bool> SavePowerConsumptionParam(JObject payload)
        {
            if (!payload.ContainsKey(TasmotaEnergyStatusKey)) return false;

            var energyModel = payload[TasmotaEnergyStatusKey].ToObject<EnergyStatusWrapperModel>()?.Energy;

            // TODO bulk processing
            foreach (var modelProperty in energyModel?.GetType()?.GetProperties())
            {
                if (modelProperty.CustomAttributes.All(att => att.AttributeType != typeof(DataMemberAttribute))) continue;
                var mappedMagnitudeKey = DataMapper.GetMapping(modelProperty.Name);
                if (!Node.ShouldMagnitudeBeStored(mappedMagnitudeKey)) continue;
                var value = modelProperty.GetValue(energyModel) as string;
                await ProcessNodeMagnitude(mappedMagnitudeKey, value);
            }
            return true;
        }
    }
}
