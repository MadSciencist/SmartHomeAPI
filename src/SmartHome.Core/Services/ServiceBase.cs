using SmartHome.Core.Utils;
using System;
using System.Security.Claims;
using Autofac;
using AutoMapper;

namespace SmartHome.Core.Services
{
    public abstract class ServiceBase : IServiceBase
    {
        protected IMapper Mapper { get; set; }
        protected ILifetimeScope Container { get; set; }
        public virtual ClaimsPrincipal Principal { get; set; }

        public ServiceBase(ILifetimeScope container, IMapper mapper)
        {
            Container = container;
            Mapper = mapper;
        }

        public ServiceBase(IMapper mapper)
        {
            Mapper = mapper;
        }

        public ServiceBase()
        {
        }

        public virtual int GetCurrentUserId(ClaimsPrincipal principal)
        {
            return Convert.ToInt32(ClaimsPrincipalHelper.GetClaimedIdentifier(principal));
        }
    }
}