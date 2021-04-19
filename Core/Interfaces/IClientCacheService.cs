using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
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