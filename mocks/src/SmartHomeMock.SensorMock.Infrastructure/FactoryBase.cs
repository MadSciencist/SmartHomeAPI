using System;
using SmartHomeMock.SensorMock.Infrastructure.Interfaces;

namespace SmartHomeMock.SensorMock.Infrastructure
{
    public class FactoryBase<TKey, TService> : IFactory<TKey, TService>
    {
        private readonly Func<TKey, TService> _serviceResolver;

        public FactoryBase(Func<TKey, TService> serviceResolver)
        {
            _serviceResolver = serviceResolver;
        }

        public TService Get(TKey key)
        {
            return _serviceResolver(key);
        }
    }
}