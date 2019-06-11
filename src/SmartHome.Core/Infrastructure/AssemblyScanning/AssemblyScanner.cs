using SmartHome.Core.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SmartHome.Core.Infrastructure.AssemblyScanning
{
    public class AssemblyScanner
    {
        protected static Type InterfaceType = typeof(IControlStrategy);

        protected virtual IEnumerable<string> GetAssemblyClassNames(string assembly)
        {
            return GetAssemblyClasses(assembly, InterfaceType).Select(x => x.Name);
        }

        protected virtual IEnumerable<Type> GetAssemblyClasses(string assembly, Type interfaceType)
        {
            return Assembly.Load(assembly)
                .GetTypes()
                .Where(x => interfaceType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);
        }
    }
}
