using SmartHome.Core.Entities.User;
using System;

namespace SmartHome.Core.Entities.Abstractions
{
    /// <summary>
    /// Represents fields, which allows to identify creation data about given entity
    /// </summary>
    public interface ICreationAudit
    {
        /// <summary>
        /// Id of user who created entity
        /// </summary>
        int CreatedById { get; set; }

        /// <summary>
        /// User who created entity
        /// </summary>
        AppUser CreatedBy { get; set; }

        /// <summary>
        /// Date of creation
        /// </summary>
        DateTime Created { get; set; }
    }
}
