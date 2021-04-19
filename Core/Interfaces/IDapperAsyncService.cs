using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDapperAsyncService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dbParams"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<List<T>> GetsAsync<T>(string sql, object dbParams = null, CommandType commandType = CommandType.Text);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dbParams"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string sql, object dbParams = null, CommandType commandType = CommandType.Text);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dbParams"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<T> InsertAsync<T>(string sql, object dbParams = null, CommandType commandType = CommandType.Text);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dbParams"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<T> UpdateAsync<T>(string sql, object dbParams = null, CommandType commandType = CommandType.Text);
    }
}