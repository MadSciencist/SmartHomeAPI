using System;
using SmartHomeMock.SensorMock.Domain.Interfaces;
using SmartHomeMock.SensorMock.Entities.Enums;
using SmartHomeMock.SensorMock.Infrastructure;

namespace SmartHomeMock.SensorMock.Domain
{
    public class ModuleMockFactory : FactoryBase<EModuleType, IModuleMock>
    {
        public ModuleMockFactory(Func<EModuleType, IModuleMock> serviceResolver) 
            : base(serviceResolver)
        {
        }
    }
}