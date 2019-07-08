﻿using SmartHome.Core.Control;
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

        public static IEnumerable<Dictionary<string, Type>> GetCommandExecutors() =>
            GetContractsAssemblies(x => typeof(IControlStrategy).IsAssignableFrom(x));

        public static IEnumerable<Dictionary<string, Type>> GetMessageHandlers()
            => GetContractsAssemblies(x => AssemblyUtils.IsAssignableToGenericType(typeof(IMessageHandler<>), x));

        private static IEnumerable<Dictionary<string, Type>> GetContractsAssemblies(Func<Type, bool> predicate)
        {
            foreach (var path in AssemblyScanner.GetContractsLibsPaths())
            {
                var asm = Assembly.LoadFile(path);
                yield return asm.GetTypes()
                    .Where(x => x.IsClass && !x.IsAbstract && !x.IsInterface)
                    .Where(predicate)
                    .ToDictionary(x => x.FullName);
            }
        }
    }
}
