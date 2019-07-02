using Microsoft.Extensions.DependencyInjection;
using SmartHome.Core.Domain.DictionaryEntity;
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

                if (!context.Dictionaries.Any(x => x.Name == "sensors"))
                {
                    var dictionaries = new List<Dictionary>
                    {
                        new Dictionary
                        {
                            Id = 10,
                            Name = "sensors",
                            Values = new List<DictionaryValue>
                            {
                                new DictionaryValue
                                {
                                    Id = 10,
                                    DisplayValue = "Generic analog",
                                    InternalValue = "analog",
                                    IsActive = true,
                                },
                                new DictionaryValue
                                {
                                    Id = 11,
                                    DisplayValue = "Generic temperature",
                                    InternalValue = "temperature",
                                    IsActive = true
                                },
                                new DictionaryValue
                                {
                                    Id = 12,
                                    DisplayValue = "Generic humidity",
                                    InternalValue = "humidity",
                                    IsActive = true
                                },
                                new DictionaryValue
                                {
                                    Id = 13,
                                    DisplayValue = "Generic voltage",
                                    InternalValue = "voltage",
                                    IsActive = true
                                },
                                new DictionaryValue
                                {
                                    Id = 14,
                                    DisplayValue = "Generic current",
                                    InternalValue = "current",
                                    IsActive = true
                                }
                            }
                        }
                    };

                    await context.Dictionaries.AddRangeAsync(dictionaries);
                    await context.SaveChangesAsync();
                }

                if (!context.Dictionaries.Any(x => x.Name == "commands"))
                {
                    var dictionaries = new List<Dictionary>
                    {
                        new Dictionary
                        {
                            Id = 1000,
                            Name = "commands",
                            Values = new List<DictionaryValue>
                            {
                                new DictionaryValue
                                {
                                    Id = 1000,
                                    DisplayValue = "Set on relay",
                                    InternalValue = "SingleRelay",
                                    IsActive = true,
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
                                    DisplayValue = "Rest",
                                    InternalValue = "Rest",
                                    IsActive = true,
                                },
                                new DictionaryValue
                                {
                                    Id = 101,
                                    DisplayValue = "Mqtt",
                                    InternalValue = "Mqtt",
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
                                    DisplayValue = "Espurna",
                                    InternalValue = "Espurna",
                                    IsActive = true,
                                },
                                new DictionaryValue
                                {
                                    Id = 201,
                                    DisplayValue = "Generic",
                                    InternalValue = "Generic",
                                    IsActive = true
                                }
                            }
                        }
                    };

                    await context.Dictionaries.AddRangeAsync(dictionaries);
                    await context.SaveChangesAsync();
                }

                if (!context.Dictionaries.Any(x => x.Name == "ReceiveProvider"))
                {
                    var dictionaries = new List<Dictionary>
                    {
                        new Dictionary
                        {
                            Id = 300,
                            Name = "ReceiveProvider",
                            Values = new List<DictionaryValue>
                            {
                                new DictionaryValue
                                {
                                    Id = 300,
                                    DisplayValue = "Rest",
                                    InternalValue = "Rest",
                                    IsActive = true,
                                },
                                new DictionaryValue
                                {
                                    Id = 301,
                                    DisplayValue = "Mqtt",
                                    InternalValue = "Mqtt",
                                    IsActive = true
                                }
                            }
                        }
                    };

                    await context.Dictionaries.AddRangeAsync(dictionaries);
                    await context.SaveChangesAsync();
                }

                if (!context.Dictionaries.Any(x => x.Name == "ReceiveContext"))
                {
                    var dictionaries = new List<Dictionary>
                    {
                        new Dictionary
                        {
                            Id = 400,
                            Name = "ReceiveContext",
                            Values = new List<DictionaryValue>
                            {
                                new DictionaryValue
                                {
                                    Id = 400,
                                    DisplayValue = "Generic",
                                    InternalValue = "Generic",
                                    IsActive = true
                                },
                                new DictionaryValue
                                {
                                    Id = 401,
                                    DisplayValue = "Espurna",
                                    InternalValue = "Espurna",
                                    IsActive = true,
                                },
                                new DictionaryValue
                                {
                                    Id = 402,
                                    DisplayValue = "Espurna with JSON payload",
                                    InternalValue = "EspurnaJsonPayload",
                                    IsActive = true,
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
