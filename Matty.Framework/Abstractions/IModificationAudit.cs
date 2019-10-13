using System;

namespace Matty.Framework.Abstractions
{
    /// <summary>
    /// Represents fields which allows to identify last updated data
    /// </summary>
    public interface IModificationAudit<TUser, TUserKey> where TUser : class, new()
    {
        /// <summary>
        /// Id of user who lastly updated entity
        /// </summary>
        TUserKey UpdatedById { get; set; }

        /// <summary>
        /// User who lastly updated entity
        /// </summary>
        TUser UpdatedBy { get; set; }

        /// <summary>
        /// Date of last update
        /// </summary>
        DateTime? Updated { get; set; }
    }
}
