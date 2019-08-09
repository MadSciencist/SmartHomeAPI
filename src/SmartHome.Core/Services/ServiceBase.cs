using Autofac;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SmartHome.Core.DataAccess;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Domain.User;
using SmartHome.Core.Utils;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public abstract class ServiceBase<TValidator, TRepository> : IServiceBase where TValidator : class, new() where TRepository : class, new()
    {
        public ClaimsPrincipal Principal { get; set; }
        protected ILifetimeScope Container { get; set; }

        private AppDbContext _context;
        protected AppDbContext DbContext => _context ?? (_context = Container.Resolve<AppDbContext>());

        private IMapper _mapper;
        protected IMapper Mapper => _mapper ?? (_mapper = Container.Resolve<IMapper>());

        private IValidator<TValidator> _validator;
        protected IValidator<TValidator> Validator => _validator ?? (_validator = Container.Resolve<IValidator<TValidator>>());

        private IGenericRepository<TRepository> _repository;
        protected IGenericRepository<TRepository> GenericRepository => _repository ?? (_repository = Container.Resolve<IGenericRepository<TRepository>>());

        private UserManager<AppUser> _userManager;
        protected UserManager<AppUser> UserManager => _userManager ?? (_userManager = Container.Resolve<UserManager<AppUser>>());

        private IConfiguration _config;
        protected IConfiguration Config => _config ?? (_config = Container.Resolve<IConfiguration>());

        #region constructor

        protected ServiceBase(ILifetimeScope container)
        {
            Container = container;
        }

        #endregion

        public async Task<AppUser> GetCurrentUser()
        {
            return await UserManager.FindByIdAsync(ClaimsPrincipalHelper.GetClaimedIdentifier(Principal));
        }

        public virtual int GetCurrentUserId()
        {
            return Convert.ToInt32(ClaimsPrincipalHelper.GetClaimedIdentifier(Principal));
        }
    }
}