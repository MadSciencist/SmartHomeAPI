using SmartHome.Core.Control;
using SmartHome.Core.MessageHandlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SmartHome.Core.Infrastructure.AssemblyScanning
{
    public class AssemblyScanner
    {
        public static IEnumerable<string> GetContractsLibsPaths()
        {
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Directory.EnumerateFiles(appDirectory, "SmartHome.Contracts.*.dll", SearchOption.AllDirectories);
        }

        public static IDictionary<string, IEnumerable<Type>> GetCommandExecutors() =>
            GetContractsAssemblies(x => typeof(IControlStrategy).IsAssignableFrom(x));

        public static IDictionary<string, IEnumerable<Type>> GetMessageHandlers()
            => GetContractsAssemblies(x => AssemblyUtils.IsAssignableToGenericType(typeof(IMessageHandler<>), x));

        private static Dictionary<string, IEnumerable<Type>> GetContractsAssemblies(Func<Type, bool> predicate)
        {
            var dict = new Dictionary<string, IEnumerable<Type>>();

            foreach (var path in GetContractsLibsPaths())
            {
                var asm = Assembly.LoadFile(path);
                var types = asm.GetTypes()
                    .Where(x => x.IsClass && !x.IsAbstract && !x.IsInterface)
                    .Where(predicate)
                    .ToList();

                dict.Add(asm.FullName, types);
            }

            return dict;
        }
    }
}
