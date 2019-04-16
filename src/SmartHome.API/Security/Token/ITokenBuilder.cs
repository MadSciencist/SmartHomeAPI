using System;
using System.Collections.Generic;
using SmartHome.Domain.User;

namespace SmartHome.API.Security.Token
{
    public interface ITokenBuilder
    {
        (string token, DateTime expring) Build(AppUser user, IEnumerable<string> roles);
    }
}
