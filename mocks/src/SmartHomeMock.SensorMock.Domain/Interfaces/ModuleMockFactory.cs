using System;
using SmartHomeMock.SensorMock.Entities.Enums;

namespace SmartHomeMock.SensorMock.Domain.Interfaces
{
    public class ModuleMockFactory : IModuleMockFactory
    {
        
        public IModuleMock GetModule(EModuleType type)
        {
            switch (type)
            {
                case EModuleType.Espurna:
                    return new EspurnaModuleMock(null);

                default:
                    throw new Exception($"Unsupported Module Type: {type}");
            }
        }
    }
}