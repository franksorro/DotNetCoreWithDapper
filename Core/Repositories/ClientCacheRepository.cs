using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using FS.Dapper;

namespace Core.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class ClientCacheRepository : IClientCacheRepository
    {
        private readonly IDapperManager dapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public ClientCacheRepository(IDapperManager dapper)
        {
            this.dapper = dapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ClientCacheModel>> GetAll()
        {
            return await dapper.SelectAsync<ClientCacheModel>(SQL_Select);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public async Task<ClientCacheModel> Get(string cacheKey)
        {
            return await dapper.SelectSingleAsync<ClientCacheModel>($"{SQL_Select} WHERE `cache_key` = @cacheKey", new
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
            return await dapper.ExecuteAsync<bool>(SQL_InsertOrUpdate, new
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
