using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHome.Core.Infrastructure.AssemblyScanning
{
    public static class ContractAssemblyAssertions
    {
        public static ILogger Logger { get; set; }

        public static void AssertValidConfig()
        {
            Logger.LogInformation("Starting Contract assemblies verification");
            VerifySingleHandlerExist();
            VerifySingleMappingExist();
            Logger.LogInformation("Contract assemblies are valid");
        }

        private static void VerifySingleMappingExist()
        {
            IDictionary<string, IEnumerable<Type>> typesDict = AssemblyScanner.GetDataMappers();

            foreach (var type in typesDict)
            {
                if (!type.Value.Any())
                {
                    var msg =
                        $"Assembly {type.Key} does not contain data mapper which implements INodeDataMapper";
                    Logger.LogError(msg);
                    throw new SmartHomeException(msg);
                }

                if (type.Value.Count() > 1)
                {
                    var msg =
                        $"Assembly {type.Key} contain more than 1 data mapper which implements INodeDataMapper";
                    Logger.LogError(msg);
                    throw new SmartHomeException(msg);
                }
            }
        }

        private static void VerifySingleHandlerExist()
        {
            IDictionary<string, IEnumerable<Type>> typesDict = AssemblyScanner.GetMessageHandlers();

            foreach (var type in typesDict)
            {
                if (!type.Value.Any())
                {
                    var msg =
                        $"Assembly {type.Key} does not contain message handler which implements IMessageHandler<>";
                    Logger.LogError(msg);
                    throw new SmartHomeException(msg);
                }

                if (type.Value.Count() > 1)
                {
                    var msg =
                        $"Assembly {type.Key} contain more than 1 message handler which implements IMessageHandler<>";
                    Logger.LogError(msg);
                    throw new SmartHomeException(msg);
                }
            }
        }
    }
}
