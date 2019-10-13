using SmartHome.Core.Entities.User;
using System;

namespace SmartHome.Core.Entities.Abstractions
{
    public interface ICreationAudit
    {
        int CreatedById { get; set; }
        AppUser CreatedBy { get; set; }
        DateTime Created { get; set; }
    }
}
