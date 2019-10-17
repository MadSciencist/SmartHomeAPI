using Autofac;
using Matty.Framework.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Entities.User;
using SmartHome.Core.Services.Abstractions;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public abstract class ServiceBase : IServiceBase
    {
        public ClaimsPrincipal Principal { get; set; }
        protected ILifetimeScope Container { get; set; }

        private IConfiguration _config;
        protected IConfiguration Config => _config ?? (_config = Container.Resolve<IConfiguration>());

        private ILogger _logger;
        protected ILogger Logger => _logger ?? (_logger = Container.Resolve<ILoggerFactory>().CreateLogger(this.GetType().FullName));

        private UserManager<AppUser> _userManager;
        protected UserManager<AppUser> UserManager => _userManager ?? (_userManager = Container.Resolve<UserManager<AppUser>>());


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