using SmartHome.Core.Control;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using SmartHome.Core.MessageHanding;

namespace SmartHome.Core.Infrastructure.AssemblyScanning
{
    public class AssemblyScanner
    {
        private const string ContractAssemblyNamingPattern = "SmartHome.Contracts.*.dll";

        public static string GetHandlerClassFullNameByAssembly(string assembly)
        {
            var typesDictionary = GetMessageHandlers();
            return typesDictionary[assembly].FirstOrDefault()?.FullName;
        }

        public static string GetMapperClassFullNameByAssembly(string assembly)
        {
            var typesDictionary = GetDataMappers();
            return typesDictionary[assembly].FirstOrDefault()?.FullName;
        }

        public static IEnumerable<string> GetContractsLibsPaths()
        {
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Directory.EnumerateFiles(appDirectory, ContractAssemblyNamingPattern, SearchOption.AllDirectories);
        }

        public static IDictionary<string, IEnumerable<Type>> GetCommandExecutors() =>
            GetContractsAssemblies(x => typeof(IControlStrategy).IsAssignableFrom(x));

        public static IDictionary<string, IEnumerable<Type>> GetMessageHandlers()
            => GetContractsAssemblies(x => AssemblyUtils.IsAssignableToGenericType(typeof(IMessageHandler<>), x));

        public static IDictionary<string, IEnumerable<Type>> GetDataMappers()
            => GetContractsAssemblies(x => typeof(INodeDataMapper).IsAssignableFrom(x));

        public static ICollection<FileVersionInfo> GetContractAssembliesInfo()
        {
            return GetContractsLibsPaths()
                .Select(Assembly.LoadFile)
                .Select(x => FileVersionInfo.GetVersionInfo(x.Location))
                .ToList();
        }

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

                var productAttribute = asm.GetAttribute<AssemblyProductAttribute>();
                dict.Add(productAttribute.Product, types);
            }

            return dict;
        }
    }
}
