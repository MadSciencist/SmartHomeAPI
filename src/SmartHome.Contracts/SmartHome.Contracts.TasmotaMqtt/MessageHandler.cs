using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartHome.Contracts.TasmotaMqtt.Domain.Models;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.ContractParams;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.MessageHanding;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Contracts.TasmotaMqtt
{
    public class Handler : MessageHandlerBase<MqttMessageDto>
    {
        public Handler(ILifetimeScope container, Node node) : base(container, node)
        {
        }

        public override async Task Handle(MqttMessageDto message)
        {
            try
            {
                var payload = JObject.Parse(message.Payload);

                // Due to tasmota complex responses, we need to check for complex types 'manually'
                if (await SaveLightbulbComplexParam(payload)) return;
                if (await SavePowerConsumptionParam(payload)) return;
                if (await SaveRelayParam(payload)) return;
            }
            catch (JsonReaderException)
            {
                // Tasmota returns confirmation twice: once as json, second time normally
                // We just want to swallow the second, redundant message exception
            }
        }

        private async Task<bool> SaveRelayParam(JObject payload)
        {
            const string relay0Key = "POWER";
            if (payload.ContainsKey(relay0Key))
            {
                var mappedProperty = DataMapper.GetMapping(relay0Key);
                if (Node.ShouldMagnitudeBeStored(mappedProperty))
                {
                    var sensorValue = payload.GetValue(relay0Key).ToString();
                    await ExtractSaveData(base.Node.Id, relay0Key, sensorValue);
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> SaveLightbulbComplexParam(JObject payload)
        {
            if (payload.ContainsKey("POWER") && payload.ContainsKey("Dimmer") && payload.ContainsKey("CT"))
            {
                if (Node.ShouldMagnitudeBeStored("light"))
                {
                    var param = new LightParam
                    {
                        State = int.Parse(ApplyConversion("relay0", payload.GetValue("POWER").ToString())),
                        Brightness = int.Parse(payload.GetValue("Dimmer").ToString()),
                        LightTemperature = int.Parse(payload.GetValue("CT").ToString())
                    };

                    var value = JsonConvert.SerializeObject(param);
                    await ExtractSaveData(Node.Id, "light", value);
                    var property = await DataMapper.GetPhysicalPropertyByContractMagnitudeAsync("light");
                    NotificationService.PushDataNotification(Node.Id,
                        new NodeDataMagnitudeDto { PhysicalProperty = property, Value = value });
                    return true;
                }
            }

            return false;
        }

        private async Task<bool> SavePowerConsumptionParam(JObject payload)
        {
            const string energyStatusKey = "StatusSNS";

            if (payload.ContainsKey(energyStatusKey))
            {
                var magnitudesDto = new List<NodeDataMagnitudeDto>();
                var obj = payload[energyStatusKey].ToObject<EnergyStatusWrapperModel>();
                var energyMap = new Dictionary<string, string>
                {
                    { "Voltage", obj.Energy.Voltage },
                    { "Current", obj.Energy.Current },
                    { "Power", obj.Energy.Power },
                    { "ReactivePower", obj.Energy.ReactivePower },
                    { "ApparentPower", obj.Energy.ApparentPower },
                    { "Factor", obj.Energy.Factor }
                };

                foreach (var (key, value) in energyMap)
                {
                    var mapped = await DataMapper.GetPhysicalPropertyByContractMagnitudeAsync(key);
                    if (Node.ShouldMagnitudeBeStored(mapped.Magnitude))
                    {
                        magnitudesDto.Add(new NodeDataMagnitudeDto(mapped, base.ApplyConversion(key, value)));
                    }
                }

                await NodeDataService.AddManyAsync(Node.Id, magnitudesDto);
                NotificationService.PushDataNotification(Node.Id, magnitudesDto);
                return true;
            }

            return false;
        }

        private async Task ExtractSaveData(int nodeId, string magnitude, string value)
        {
            var property = await DataMapper.GetPhysicalPropertyByContractMagnitudeAsync(magnitude);

            // Check if there is associated system property
            if (property is null) return;

            var magnitudeDto = new NodeDataMagnitudeDto
            {
                Value = base.ApplyConversion(property.Magnitude, value),
                PhysicalProperty = property
            };

            await NodeDataService.AddSingleAsync(nodeId, magnitudeDto);
            NotificationService.PushDataNotification(nodeId, magnitudeDto);
        }
    }
}
