using SmartHome.Core.Domain.DictionaryEntity;
using SmartHome.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Infrastructure.SyntheticDictionaries
{
    // TODO refactor, some caching?
    public class SyntheticDictionaryService
    {
        public ICollection<Dictionary> Dictionaries { get; private set; }

        private readonly IControlStrategyService _strategyService;

        public SyntheticDictionaryService(IControlStrategyService strategyService)
        {
            _strategyService = strategyService;
        }

        public void Initialize()
        {
            FillDictionaries().Wait();
        }

        private async Task FillDictionaries()
        {
            var controlStrategies = await _strategyService.GetAll();

            Dictionaries = Dictionaries ?? new List<Dictionary>();
            Dictionaries.Add(new Dictionary
            {
                Name = "ControlStrategy",
                Values = controlStrategies.Data.Select(x => new DictionaryValue
                {
                    DisplayValue = x.Description,
                    InternalValue = x.Id.ToString(),
                    Id = x.Id
                }).ToList()
            });
        }

        public bool HasDictionary(string name)
        {
            return Dictionaries.Any(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<string> GetNames()
        {
            return Dictionaries.Select(x => x.Name);
        }

        public Dictionary GetDictionary(string name)
        {
            return Dictionaries.SingleOrDefault(x =>
                string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
