using System;
using System.Collections.Generic;
using SmartHome.Core.Entities.User;

namespace SmartHome.Core.Security
{
    public interface ITokenBuilder
    {
        (string token, DateTime expring) Build(AppUser user, IEnumerable<string> roles);
    }
}
