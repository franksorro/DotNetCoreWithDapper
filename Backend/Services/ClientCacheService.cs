using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Options;
using Backend.Models;
using Backend.Middlewares;
using Backend.Interfaces;
using Backend.Extensions;

namespace Backend.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ClientCacheService : IClientCacheService
    {
        private readonly IClientCacheRepository clientCacheRepository;
        private readonly IMemCachedService memCachedService;
        private readonly AppSettings settings;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientCacheRepository"></param>
        /// <param name="settings"></param>
        /// <param name="memCachedService"></param>
        public ClientCacheService(
            IClientCacheRepository clientCacheRepository,
            IOptions<AppSettings> settings,
            IMemCachedService memCachedService)
        {
            this.clientCacheRepository = clientCacheRepository;
            this.memCachedService = memCachedService;
            this.settings = settings.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<ClientCacheModel> Get(string key)
        {
            if (settings.MemCached.Enabled)
                return new ClientCacheModel
                {
                    CacheKey = key,
                    CacheValue = await memCachedService.Get<long>(key)
                };

            ClientCacheModel clientCacheModel = await clientCacheRepository.Get(key);

            if (settings.MemCached.Enabled)
                _ = await memCachedService.Set(clientCacheModel.CacheKey, clientCacheModel.CacheValue);

            return clientCacheModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> Set(string key)
        {
            long cacheValue = DateTime.UtcNow.ToLongFrom1970();

            if (settings.MemCached.Enabled)
                _ = await memCachedService.Set(key, cacheValue);

            return await clientCacheRepository.Set(key, cacheValue);
        }
    }
}