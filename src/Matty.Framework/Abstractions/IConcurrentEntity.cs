using System.ComponentModel.DataAnnotations;

namespace Matty.Framework.Abstractions
{
    /// <summary>
    /// Interface that is used to mark entity concurrency-aware.
    /// </summary>
    public interface IConcurrentEntity
    {
        /// <summary>
        /// RowVersion for concurrency check.
        /// </summary>
        [Timestamp]
        [ConcurrencyCheck]
        byte[] RowVersion { get; set; }
    }
}
