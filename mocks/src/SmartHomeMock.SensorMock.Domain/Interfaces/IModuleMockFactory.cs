using SmartHomeMock.SensorMock.Entities.Enums;

namespace SmartHomeMock.SensorMock.Domain.Interfaces
{
    public interface IModuleMockFactory
    {
        IModuleMock GetModule(EModuleType type);
    }
}