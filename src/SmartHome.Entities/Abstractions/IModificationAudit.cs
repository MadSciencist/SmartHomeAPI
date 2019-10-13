using SmartHome.Core.Entities.User;
using System;

namespace SmartHome.Core.Entities.Abstractions
{
    public interface IModificationAudit
    {
        int? UpdatedById { get; set; }
        AppUser UpdatedBy { get; set; }
        DateTime? Updated { get; set; }
    }
}
