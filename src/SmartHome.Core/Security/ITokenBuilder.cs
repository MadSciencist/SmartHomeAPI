using SmartHome.Core.Entities.User;
using System;
using System.Collections.Generic;

namespace SmartHome.Core.Security
{
    public interface ITokenBuilder
    {
        (string token, DateTime expring) Build(AppUser user, IEnumerable<string> roles);
    }
}
