using System.Linq;
using System.Reflection;
using Autofac;

namespace SmartHome.Core.Contracts.Rest.Control.Extensions
{
    public static class IoCExtensions
    {
        /// <summary>
        /// Register all the implementation as named instanced into IoC container
        /// They can be resolved using fully qualified name
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterRestNodeContracts(this ContainerBuilder builder)
        {
            var commandExecutorAsm = Assembly.GetAssembly(typeof(IRestControlStrategy)).GetTypes();

            var contracts = commandExecutorAsm.ToDictionary(ex => ex.FullName);

            foreach (var contract in contracts)
            {
                builder.RegisterType(contract.Value)
                    .Named<object>(contract.Key);
            }
        }
    }
}
