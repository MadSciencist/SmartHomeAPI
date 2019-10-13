using System;

namespace Matty.Framework.Abstractions
{
    /// <summary>
    /// Represents fields, which allows to identify creation data about given entity
    /// </summary>
    public interface ICreationAudit<TUser, TUserKey> where TUser : class, new()
    {
        /// <summary>
        /// Id of user who created entity
        /// </summary>
        TUserKey CreatedById { get; set; }

        /// <summary>
        /// User who created entity
        /// </summary>
        TUser CreatedBy { get; set; }

        /// <summary>
        /// Date of creation
        /// </summary>
        DateTime Created { get; set; }
    }
}
