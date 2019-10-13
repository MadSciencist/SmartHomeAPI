using SmartHome.Core.Entities.User;
using System;

namespace SmartHome.Core.Entities.Abstractions
{
    /// <summary>
    /// Represents fields which allows to identify last updated data
    /// </summary>
    public interface IModificationAudit
    {
        /// <summary>
        /// Id of user who lastly updated entity
        /// </summary>
        int? UpdatedById { get; set; }

        /// <summary>
        /// User who lastly updated entity
        /// </summary>
        AppUser UpdatedBy { get; set; }

        /// <summary>
        /// Date of last update
        /// </summary>
        DateTime? Updated { get; set; }
    }
}
