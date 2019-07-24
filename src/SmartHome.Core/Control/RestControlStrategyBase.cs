using Autofac;
using SmartHome.Core.RestClient;

namespace SmartHome.Core.Control
{
    public abstract class RestControlStrategyBase : ControlStrategyBase
    {
        protected RestControlStrategyBase(ILifetimeScope container) : base(container)
        {
        }

        private PersistentHttpClient _httpClient;
        protected PersistentHttpClient HttpClient =>
            _httpClient ?? (_httpClient = Container.Resolve<PersistentHttpClient>());
    }
}
