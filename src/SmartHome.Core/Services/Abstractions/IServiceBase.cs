﻿using SmartHome.Core.Entities.User;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartHome.Core.Services.Abstractions
{
    public interface IServiceBase
    {
        ClaimsPrincipal Principal { get; set; }
        int GetCurrentUserId();
        Task<AppUser> GetCurrentUser();
    }
}