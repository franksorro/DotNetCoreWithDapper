using Backend.Interfaces;
using Enyim.Caching;
using System.Threading.Tasks;

namespace Backend.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class MemCachedService : IMemCachedService
    {
        private readonly IMemcachedClient memCachedClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memCachedClient"></param>
        public MemCachedService(IMemcachedClient memCachedClient)
        {
            this.memCachedClient = memCachedClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> Get<T>(string key)
        {
            return await memCachedClient.GetValueAsync<T>(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheDuration"></param>
        /// <returns></returns>
        public async Task<bool> Set<T>(string key, T value, int cacheDuration = 300)
        {
            return await memCachedClient.SetAsync(key, value, cacheDuration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> Remove(string key)
        {
            return await memCachedClient.RemoveAsync(key);
        }
    }
}
