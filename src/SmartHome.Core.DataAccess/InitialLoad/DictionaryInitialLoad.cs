﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartHome.Core.Domain.DictionaryEntity;

namespace SmartHome.Core.DataAccess.InitialLoad
{
    public class DictionaryInitialLoad
    {
        public static async Task Seed(IServiceProvider provider)
        {
            using (var scope = provider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

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
                                    Value = "Male",
                                    IsActive = true,
                                },
                                new DictionaryValue
                                {
                                    Id = 2,
                                    Value = "Female",
                                    IsActive = true
                                }
                            }
                        }
                    };

                    await context.Dictionaries.AddRangeAsync(dictionaries);
                    await context.SaveChangesAsync();
                }

                if (!context.Dictionaries.Any(x => x.Name == "ControlProvider"))
                {
                    var dictionaries = new List<Dictionary>
                    {
                        new Dictionary
                        {
                            Id = 100,
                            Name = "ControlProvider",
                            Values = new List<DictionaryValue>
                            {
                                new DictionaryValue
                                {
                                    Id = 100,
                                    Value = "Rest",
                                    IsActive = true,
                                },
                                new DictionaryValue
                                {
                                    Id = 101,
                                    Value = "Mqtt",
                                    IsActive = true
                                }
                            }
                        }
                    };

                    await context.Dictionaries.AddRangeAsync(dictionaries);
                    await context.SaveChangesAsync();
                }

                if (!context.Dictionaries.Any(x => x.Name == "ControlContext"))
                {
                    var dictionaries = new List<Dictionary>
                    {
                        new Dictionary
                        {
                            Id = 200,
                            Name = "ControlContext",
                            Values = new List<DictionaryValue>
                            {
                                new DictionaryValue
                                {
                                    Id = 200,
                                    Value = "Espurna",
                                    IsActive = true,
                                },
                                new DictionaryValue
                                {
                                    Id = 201,
                                    Value = "Custom",
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