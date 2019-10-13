using Microsoft.Extensions.DependencyInjection;
using SmartHome.Core.Entities.DictionaryEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.DataAccess.InitialLoad
{
    public class DictionaryInitialLoad
    {
        public static async Task Seed(IServiceProvider provider)
        {
            using (var scope = provider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<EntityFrameworkContext>();

                if (!context.Dictionaries.Any(x => x.Name == "gender"))
                {
                    var dictionaries = new List<Dictionary>
                    {
                        new Dictionary
                        {
                            Id = 1,
                            Name = "gender",
                            Values = new List<DictionaryValue>
                            {
                                new DictionaryValue
                                {
                                    Id = 1,
                                    DisplayValue = "Male",
                                    InternalValue = "Male",
                                    IsActive = true,
                                },
                                new DictionaryValue
                                {
                                    Id = 2,
                                    DisplayValue = "Female",
                                    InternalValue = "Female",
                                    IsActive = true
                                }
                            }
                        }
                    };

                    await context.Dictionaries.AddRangeAsync(dictionaries);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
