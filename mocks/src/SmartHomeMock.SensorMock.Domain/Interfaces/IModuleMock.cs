using System.Threading.Tasks;
using SmartHomeMock.SensorMock.Entities.Configuration;

namespace SmartHomeMock.SensorMock.Domain.Interfaces
{
    public interface IModuleMock
    {
        void Initialize(Module module, Broker broker);

        Task Start();
    }
}