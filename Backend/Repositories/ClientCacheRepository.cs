using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class ClientCacheRepository : IClientCacheRepository
    {
        private readonly IDapperAsyncService service;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public ClientCacheRepository(IDapperAsyncService service)
        {
            this.service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ClientCacheModel>> GetAll()
        {
            return await service.GetsAsync<ClientCacheModel>(SQL_Select);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public async Task<ClientCacheModel> Get(string cacheKey)
        {
            return await service.GetAsync<ClientCacheModel>($"{SQL_Select} WHERE `cache_key` = @cacheKey", new
            {
                cacheKey
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        /// <returns></returns>
        public async Task<bool> Set(string cacheKey, long cacheValue)
        {
            return await service.InsertAsync<bool>(SQL_InsertOrUpdate, new
            {
                cacheKey,
                cacheValue
            });
        }

        #region Queries
        private readonly string SQL_Select = @"
        SELECT
            `cache_key` AS `cacheKey`,
            `cache_value` AS `cacheValue`
        FROM `clients_cache`";

        private readonly string SQL_InsertOrUpdate = @"
        INSERT INTO `clients_cache`
        (
            `cache_key`,
            `cache_value`
        ) VALUES (
            @cacheKey,
            @cacheValue
        )
        ON DUPLICATE KEY UPDATE
            `cache_value` = @cacheValue";
        #endregion
    }
}
