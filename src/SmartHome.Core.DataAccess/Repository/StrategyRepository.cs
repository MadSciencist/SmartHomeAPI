﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Domain.Entity;
using System.Threading.Tasks;

namespace SmartHome.Core.DataAccess.Repository
{
    public class StrategyRepository : GenericRepository<ControlStrategy>, IStrategyRepository
    {
        private readonly ILogger _logger;

        public StrategyRepository(AppDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(StrategyRepository));
        }

        public override async Task<ControlStrategy> GetByIdAsync(int id)
        {
            return await _context.ControlStrategies
                .Include(x => x.AllowedCommands)
                    .ThenInclude(x => x.Command)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}