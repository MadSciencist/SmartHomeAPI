using System;
using System.Collections.Generic;
using SmartHome.Core.Domain.User;

namespace SmartHome.API.Security.Token
{
    public interface ITokenBuilder
    {
        (string token, DateTime expring) Build(AppUser user, IEnumerable<string> roles);
    }
}
