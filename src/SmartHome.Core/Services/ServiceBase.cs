using SmartHome.Core.Utils;
using System;
using System.Security.Claims;
using Autofac;
using AutoMapper;
using FluentValidation;
using SmartHome.Core.DataAccess.Repository;

namespace SmartHome.Core.Services
{
    public abstract class ServiceBase<TValidator, TRepository> : IServiceBase where TValidator: class, new() where TRepository: class, new()
    {
        public virtual ClaimsPrincipal Principal { get; set; }
        protected IMapper Mapper { get; set; }
        protected ILifetimeScope Container { get; set; }
        protected IValidator<TValidator> Validator { get; set; }
        protected IGenericRepository<TRepository> GenericRepository { get; set; }

        #region constructors
        protected ServiceBase(ILifetimeScope container, IMapper mapper, IValidator<TValidator> validator, IGenericRepository<TRepository> repository)
        {
            Container = container;
            Mapper = mapper;
            Validator = validator;
            GenericRepository = repository;
        }

        protected ServiceBase(ILifetimeScope container, IMapper mapper, IValidator<TValidator> validator)
        {
            Container = container;
            Mapper = mapper;
            Validator = validator;
        }

        protected ServiceBase(IGenericRepository<TRepository> repository, IMapper mapper, IValidator<TValidator> validator)
        {
            GenericRepository = repository;
            Mapper = mapper;
            Validator = validator;
        }

        protected ServiceBase(IMapper mapper, IValidator<TValidator> validator)
        {
            Mapper = mapper;
            Validator = validator;
        }

        protected ServiceBase(IMapper mapper)
        {
            Mapper = mapper;
        }

        protected ServiceBase(IGenericRepository<TRepository> repository)
        {
            GenericRepository = repository;
        }

        protected ServiceBase()
        {
        }

        #endregion

        public virtual int GetCurrentUserId()
        {
            return Convert.ToInt32(ClaimsPrincipalHelper.GetClaimedIdentifier(Principal));
        }
    }
}