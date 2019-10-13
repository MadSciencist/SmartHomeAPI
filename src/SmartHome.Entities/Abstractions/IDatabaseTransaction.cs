using System;

namespace SmartHome.Core.Entities.Abstractions
{
    /// <summary>
    /// Represents database transaction
    /// </summary>
    public interface IDatabaseTransaction : IDisposable
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
