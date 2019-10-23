using Autofac;
using Matty.Framework;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHome.Core.Repositories;

namespace SmartHome.Core.Services
{
    public class ControlStrategyService : ServiceBase, IControlStrategyService
    {
        private readonly IControlStrategyRepository _strategyRepository;

        public ControlStrategyService(ILifetimeScope container, IControlStrategyRepository strategyRepository) : base(container)
        {
            _strategyRepository = strategyRepository;
        }

        public async Task<ServiceResult<IEnumerable<ControlStrategy>>> GetAll()
        {
            var strategies = await _strategyRepository.GetAllAsync();

            var result = new ServiceResult<IEnumerable<ControlStrategy>>(Principal)
            {
                Data = strategies
            };

            return result;
        }
    }
}
