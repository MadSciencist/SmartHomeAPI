using FluentValidation;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Dto;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Infrastructure.Validators
{
    public class NodeDtoValidator : AbstractValidator<NodeDto>
    {
        private readonly INodeRepository _nodeRepository;

        public NodeDtoValidator(INodeRepository nodeRepository)
        {
            _nodeRepository = nodeRepository;

            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.ClientId)
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(250);

            RuleFor(x => x.ClientId)
                .MustAsync((x, cancellation) => CheckIfClientIdIsUnique(x))
                .WithMessage("{PropertyName} must be unique");
        }

        public async Task<bool> CheckIfClientIdIsUnique(string clientId)
        {
            var existingClientIds = await _nodeRepository.GetAllClientIdsAsync();
            return !existingClientIds.Contains(clientId);
        }
    }
}
