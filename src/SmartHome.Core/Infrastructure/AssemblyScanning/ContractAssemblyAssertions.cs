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
            VerifySingleHandler();
            VerifyMappingExist();
            Logger.LogInformation("Contract assemblies are valid");
        }

        // TODO
        private static void VerifyMappingExist()
        {
        }

        private static void VerifySingleHandler()
        {
            IDictionary<string, IEnumerable<Type>> handlerAssemblies = AssemblyScanner.GetMessageHandlers();

            foreach (var assembly in handlerAssemblies)
            {
                if (!assembly.Value.Any())
                {
                    var msg =
                        $"Assembly {assembly.Key} does not contain message handler which implements IMessageHandler<>";
                    Logger.LogError(msg);
                    throw new SmartHomeException(msg);
                }

                if (assembly.Value.Count() > 1)
                {
                    var msg =
                        $"Assembly {assembly.Key} contain more than 1 message handler which implements IMessageHandler<>";
                    Logger.LogError(msg);
                    throw new SmartHomeException(msg);
                }
            }
        }
    }
}
