using Autofac;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Data.Repository;
using System.Threading.Tasks;

namespace SmartHome.Core.MessageHanding
{
    public abstract class MessageProcessorBase<T> : IMessageProcessor<T> where T : class, new()
    {
        protected readonly ILifetimeScope Container;

        private INodeRepository _nodeRepository;

        protected INodeRepository NodeRepository =>
            _nodeRepository ?? (_nodeRepository = Container.Resolve<INodeRepository>());

        protected ILogger Logger => _logger ?? (_logger = Container.Resolve<ILogger<MessageProcessorBase<T>>>());
        private ILogger _logger;

        protected MessageProcessorBase(ILifetimeScope container)
        {
            Container = container;
        }

        public abstract Task Process(T message);
    }
}
