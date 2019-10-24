using FluentValidation;
using SmartHome.Core.Dto;
using SmartHome.Core.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Infrastructure.Validators
{
    public class NodeDtoValidator : AbstractValidator<NodeDto>
    {
        private readonly INodeRepository _nodeRepository;
        private readonly IControlStrategyRepository _strategyRepository;

        public NodeDtoValidator(INodeRepository nodeRepository, IControlStrategyRepository strategyRepository)
        {
            _nodeRepository = nodeRepository;
            _strategyRepository = strategyRepository;

            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Name)
                .NotEmpty();

            WhenAsync((node, ct) => ShouldValidateClientId(node), () =>
            {
                RuleFor(x => x.ClientId)
                    .MustAsync(async (x, ct) => await CheckIfClientIdIsUnique(x))
                    .WithMessage("{PropertyName} must be unique");
            });
        }

        // Validation of ClientId only make sense for MQTT-based devices
        private async Task<bool> ShouldValidateClientId(NodeDto node)
        {
            // Update case - we got ID, and already assigned ClientId
            if (node.Id > 0 && !string.IsNullOrEmpty(node.ClientId)) return false;

            var strategy = await _strategyRepository.GetByIdAsync(node.ControlStrategyId);
            return strategy.Connector.ToLower().Contains("mqtt");
        }

        private async Task<bool> CheckIfClientIdIsUnique(string clientId)
        {
            var existingClientIds = await _nodeRepository.GetAllClientIdsAsync();
            return !existingClientIds.Contains(clientId);
        }
    }
}
