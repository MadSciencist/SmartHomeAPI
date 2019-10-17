using Autofac;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Services.Abstractions;
using System;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public class PhysicalPropertyService : ServiceBase, IPhysicalPropertyService
    {
        public PhysicalPropertyService(ILifetimeScope container) : base(container)
        {
        }

        public Task<PhysicalProperty> GetByMagnitudeAsync(string magnitude)
        {
            return Task.FromResult(new PhysicalProperty());
        }
    }
}
