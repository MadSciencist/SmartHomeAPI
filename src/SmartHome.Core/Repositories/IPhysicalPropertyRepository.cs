﻿using Matty.Framework.Abstractions;
using SmartHome.Core.Entities.Entity;

namespace SmartHome.Core.Repositories
{
    public interface IPhysicalPropertyRepository : ITransactionalRepository<PhysicalProperty, int>
    {
    }
}
