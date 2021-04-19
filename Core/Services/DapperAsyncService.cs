using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class DapperAsyncService : IDapperAsyncService
    {
        private readonly IDapperCore dapperCore;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dapperCore"></param>
        public DapperAsyncService(IDapperCore dapperCore)
        {
            this.dapperCore = dapperCore;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sp"></param>
        /// <param name="dbParams"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async Task<List<T>> GetsAsync<T>(string sql, object dbParams = null, CommandType commandType = CommandType.Text)
        {
            return await Task.FromResult(dapperCore.GetAll<T>(sql, dbParams, commandType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sp"></param>
        /// <param name="dbParams"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string sql, object dbParams = null, CommandType commandType = CommandType.Text)
        {
            return await Task.FromResult(dapperCore.Get<T>(sql, dbParams, commandType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dbParams"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async Task<T> InsertAsync<T>(string sql, object dbParams = null, CommandType commandType = CommandType.Text)
        {
            return await Task.FromResult(dapperCore.Insert<T>(sql, dbParams, commandType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dbParams"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async Task<T> UpdateAsync<T>(string sql, object dbParams = null, CommandType commandType = CommandType.Text)
        {
            return await Task.FromResult(dapperCore.Update<T>(sql, dbParams, commandType));
        }
    }
}
