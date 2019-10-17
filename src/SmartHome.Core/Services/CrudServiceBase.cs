using Autofac;
using AutoMapper;
using FluentValidation;
using Matty.Framework.Abstractions;

namespace SmartHome.Core.Services
{
    public class CrudServiceBase<TValidator, TRepository> : ServiceBase where TValidator : class, new() where TRepository : class, IEntity, new()
    {
        private IMapper _mapper;
        protected IMapper Mapper
            => _mapper ?? (_mapper = Container.Resolve<IMapper>());

        private IValidator<TValidator> _validator;
        protected IValidator<TValidator> Validator
            => _validator ?? (_validator = Container.Resolve<IValidator<TValidator>>());

        private IGenericRepository<TRepository> _repository;
        protected IGenericRepository<TRepository> GenericRepository
            => _repository ?? (_repository = Container.Resolve<IGenericRepository<TRepository>>());


        #region constructor

        protected CrudServiceBase(ILifetimeScope container) : base(container)
        {
        }

        #endregion
    }
}
