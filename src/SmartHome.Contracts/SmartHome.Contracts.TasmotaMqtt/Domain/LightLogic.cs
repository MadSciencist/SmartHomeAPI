using SmartHome.Core.Domain.ContractParams;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHome.Contracts.TasmotaMqtt.Domain
{
    internal class LightLogic
    {
        private readonly BacklogBuilder _builder = new BacklogBuilder();
        private const int MinLightTemperature = 153;
        private const int MaxLightTemperature = 500;

        public (string topic, string payload) GetTopicPayloadForLight(LightParam param)
        { 
            if (param is null) throw new ArgumentNullException();

            // Only state is given
            if (param.State.HasValue && !param.LightTemperature.HasValue && !param.Brightness.HasValue)
            {
                if (new[] { 0, 1, 2 }.Contains(param.State.Value))
                    return ("power", param.State.Value.ToString());
            }

            // Only Brightness is given
            if (param.Brightness.HasValue && !param.State.HasValue && !param.LightTemperature.HasValue)
            {
                var val = param.Brightness.Value;
                if (val >= 0 && val <= 100) return ("dimmer", val.ToString());
            }

            // Only LightTemperature is given
            if (param.LightTemperature.HasValue && !param.Brightness.HasValue && !param.State.HasValue)
            {
                var val = param.LightTemperature.Value;
                if (val >= 0 && val <= 100)
                    return ("ct", ConvertRange(val, 0, 100, MinLightTemperature, MaxLightTemperature).ToString());
            }

            // State and Brightness are given
            if (param.State.HasValue && param.Brightness.HasValue && !param.LightTemperature.HasValue)
            {
                // Power off or toggle overrides others
                if (new[] { 0, 2 }.Contains(param.State.Value))
                    return ("power", param.State.Value.ToString());

                var backlog = _builder.GetBacklog(new Dictionary<string, string>
                {
                    {"power", param.State.ToString()},
                    {"dimmer", param.Brightness.ToString()}
                });

                return backlog;
            }

            // State and LightTemperature are given
            if (param.State.HasValue && param.LightTemperature.HasValue && !param.Brightness.HasValue)
            {
                // Power off or toggle overrides others
                if (new[] { 0, 2 }.Contains(param.State.Value))
                    return ("power", param.State.Value.ToString());

                var backlog = _builder.GetBacklog(new Dictionary<string, string>
                {
                    {"power", param.State.ToString()},
                    {"ct", ConvertRange(param.LightTemperature.Value, 0, 100, MinLightTemperature, MaxLightTemperature).ToString()}
                });
                
                return backlog;
            }


            // All parameters are given
            if (param.LightTemperature.HasValue && param.Brightness.HasValue && param.State.HasValue)
            {
                // Power off or toggle overrides others
                if (new[] { 0, 2 }.Contains(param.State.Value))
                    return ("power", param.State.Value.ToString());

                var backlog = _builder.GetBacklog(new Dictionary<string, string>
                {
                    {"power", param.State.ToString()},
                    {"dimmer", param.Brightness.ToString()},
                    {"ct", ConvertRange(param.LightTemperature.Value, 0, 100, MinLightTemperature, MaxLightTemperature).ToString()}
                });

                return backlog;
            }

            throw new ArgumentException($"Invalid parameter {nameof(LightParam)}");
        }

        public (string topic, string payload) GetTopicPayloadForRgbLight(RgbLightParam param)
        {
            if (param is null) throw new ArgumentNullException();

            // Only state is given
            if (param.State.HasValue && param.Rgb is null)
            {
                if (new[] { 0, 1, 2 }.Contains(param.State.Value))
                    return ("power", param.State.Value.ToString());
            }

            // Only RGB
            if (param.Rgb != null && !param.State.HasValue)
            {
                for(var i = 0; i < 3; i++)
                    if (param.Rgb[i] > 255)
                        throw new ArgumentException($"Parameter RGB is not in given range, {nameof(RgbLightParam)}");
                return ("color", $"{param.Rgb[0].ToString()}, {param.Rgb[1].ToString()}, {param.Rgb[2].ToString()}");
            }

            // State and RGB
            if (param.Rgb != null && param.State.HasValue)
            {
                // Power off or toggle overrides others
                if (new[] { 0, 2 }.Contains(param.State.Value))
                    return ("power", param.State.Value.ToString());

                for (var i = 0; i < 3; i++)
                    if (param.Rgb[i] > 255)
                        throw new ArgumentException($"Parameter RGB is not in given range, {nameof(RgbLightParam)}");

                return _builder.GetBacklog(new Dictionary<string, string>
                {
                    {"power", param.State.ToString()},
                    {"color", $"{param.Rgb[0].ToString()}, {param.Rgb[1].ToString()}, {param.Rgb[2].ToString()}"}
                });
            }

            throw new ArgumentException($"Invalid parameter {nameof(LightParam)}");
        }

        private static int ConvertRange(int x, int inMin, int inMax, int outMin, int outMax)
        {
            return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }
    }
}
