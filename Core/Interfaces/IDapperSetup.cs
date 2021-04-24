using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDapperSetup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dbParams"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAll<T>(string sql, object dbParams = null, CommandType commandType = CommandType.Text);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dbParams"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<T> Get<T>(string sql, object dbParams = null, CommandType commandType = CommandType.Text);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dbParams"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<T> Insert<T>(string sql, object dbParams = null, CommandType commandType = CommandType.Text);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dbParams"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<T> Execute<T>(string sql, object dbParams = null, CommandType commandType = CommandType.Text);
    }
}