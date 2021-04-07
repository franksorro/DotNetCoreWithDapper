using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Interfaces;
using Backend.Middlewares;
using Backend.Models;
using Backend.Types;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Backend.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class TestService
    {
        private readonly ITestRepository repo;
        private readonly IMemCachedService memCachedService;
        private readonly IClientCacheService clientCacheService;
        private readonly AppSettings settings;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="memCachedService"></param>
        /// <param name="clientCacheService"></param>
        /// <param name="settings"></param>
        public TestService(
            ITestRepository repo,
            IMemCachedService memCachedService,
            IClientCacheService clientCacheService,
            IOptions<AppSettings> settings)
        {
            this.repo = repo;
            this.memCachedService = memCachedService;
            this.clientCacheService = clientCacheService;
            this.settings = settings.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TestModel>> GetAll()
        {
            if (settings.MemCached.Enabled)
            {
                string cacheKey = MemCachedType.Test.GetAll;
                string cacheValue = await memCachedService.Get<string>(cacheKey);

                IEnumerable<TestModel> models;

                if (!string.IsNullOrEmpty(cacheValue))
                {
                    models = JsonConvert.DeserializeObject<IEnumerable<TestModel>>(cacheValue);
                }
                else
                {
                    models = await repo.GetAll();
                    _ = await memCachedService.Set(cacheKey, JsonConvert.SerializeObject(models));
                }

                return models;
            }
            
            return await repo.GetAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TestModel> Get(int id)
        {
            if (settings.MemCached.Enabled)
            {
                TestModel model;
                string cacheKey = $"{MemCachedType.Test.Get}{id}";
                string cacheValue = await memCachedService.Get<string>(cacheKey);

                if (!string.IsNullOrEmpty(cacheValue))
                {
                    model = JsonConvert.DeserializeObject<TestModel>(cacheValue);
                }
                else
                {
                    model = await repo.Get(id);
                    _ = await memCachedService.Set(cacheKey, JsonConvert.SerializeObject(model));
                }

                return model;
            }
            
            return await repo.Get(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> Add(TestModel model)
        {
            bool result = await repo.Add(model);

            if (result)
            {
                if (settings.MemCached.Enabled)
                    _ = await memCachedService.Remove(MemCachedType.Test.GetAll);

                if (settings.ClientCache.Enabled)
                    await clientCacheService.Set(ClientCacheType.Test.GetAll);

                return true;
            }
            
            return result;
        }
    }
}
