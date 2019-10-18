using System;

namespace Matty.Framework.Abstractions
{
    /// <summary>
    /// Represents database transaction
    /// </summary>
    public interface ITransaction : IDisposable
    {
        /// <summary>
        /// Unique transaction identifier
        /// </summary>
        Guid Identifier { get; }

        /// <summary>
        /// Save transaction
        /// </summary>
        void Commit();

        /// <summary>
        /// Undo transaction
        /// </summary>
        void Rollback();
    }
}
