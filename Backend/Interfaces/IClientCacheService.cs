using Backend.Models;
using System.Threading.Tasks;

namespace Backend.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IClientCacheService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<ClientCacheModel> Get(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> Set(string key);
    }
}