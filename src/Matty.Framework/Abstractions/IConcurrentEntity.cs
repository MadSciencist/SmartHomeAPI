using System.ComponentModel.DataAnnotations;

namespace Matty.Framework.Abstractions
{
    public interface IConcurrentEntity
    {
        [Timestamp]
        [ConcurrencyCheck]
        byte[] RowVersion { get; set; }
    }
}
