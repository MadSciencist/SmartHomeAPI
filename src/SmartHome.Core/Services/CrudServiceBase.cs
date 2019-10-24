using Autofac;
using AutoMapper;
using FluentValidation;
using Matty.Framework.Abstractions;

namespace SmartHome.Core.Services
{
    public class CrudServiceBase<TValidator, TRepository> : ServiceBase where TValidator : class, new() where TRepository : class, IEntity, new()
    {
        private IValidator<TValidator> _validator;
        protected IValidator<TValidator> Validator
            => _validator ??= Container.Resolve<IValidator<TValidator>>();

        private ITransactionalRepository<TRepository, int> _repository;
        protected ITransactionalRepository<TRepository, int> GenericRepository
            => _repository ??= Container.Resolve<ITransactionalRepository<TRepository, int>>();


        #region constructor

        protected CrudServiceBase(ILifetimeScope container) : base(container)
        {
        }

        #endregion
    }
}
