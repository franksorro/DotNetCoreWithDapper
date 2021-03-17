using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Interfaces;
using Backend.Models;
using Backend.Types;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public TestService(
            ITestRepository repo,
            IMemCachedService memCachedService)
        {
            this.repo = repo;
            this.memCachedService = memCachedService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TestModel>> GetAll()
        {
            string cacheKey = MemCachedType.GetAll;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TestModel> Get(int id)
        {
            string cacheKey = $"{MemCachedType.Get}{id}";
            string cacheValue = await memCachedService.Get<string>(cacheKey);

            TestModel model;

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
    }
}
