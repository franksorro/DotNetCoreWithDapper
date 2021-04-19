using Core.Interfaces;
using Core.Middlewares;
using Enyim.Caching;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Core.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class MemCachedService : IMemCachedService
    {
        private readonly IMemcachedClient memCachedClient;
        private readonly AppSettings settings;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memCachedClient"></param>
        /// <param name="settings"></param>
        public MemCachedService(
            IMemcachedClient memCachedClient,
            IOptions<AppSettings> settings)
        {
            this.memCachedClient = memCachedClient;
            this.settings = settings.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> Get<T>(string key)
        {
            if (!settings.MemCached.Enabled)
                return default;

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
            if (!settings.MemCached.Enabled)
                return false;

            return await memCachedClient.SetAsync(key, value, cacheDuration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> Remove(string key)
        {
            if (!settings.MemCached.Enabled)
                return false;

            return await memCachedClient.RemoveAsync(key);
        }
    }
}
