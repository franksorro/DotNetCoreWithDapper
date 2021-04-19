using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ClientCacheModel
    {
        /// <summary>
        /// Chiave cache
        /// </summary>
        [Key]
        public string CacheKey { get; set; }

        /// <summary>
        /// Valore del timestamp
        /// </summary>
        public long CacheValue { get; set; }
    }
}