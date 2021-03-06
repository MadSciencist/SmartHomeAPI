﻿using Matty.Framework.Abstractions;
using SmartHome.Core.Entities.Entity;

namespace SmartHome.Core.Repositories
{
    public interface IStrategyRepository : ITransactionalRepository<ControlStrategy, int>
    {
    }
}
