using System.Threading.Tasks;

namespace Backend.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMemCachedService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> Get<T>(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheDuration"></param>
        /// <returns></returns>
        Task<bool> Set<T>(string key, T value, int cacheDuration = 300);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> Remove(string key);
    }
}