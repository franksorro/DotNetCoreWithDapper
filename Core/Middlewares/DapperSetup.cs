using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using Core.Interfaces;
using System.Threading.Tasks;

namespace Core.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    public class DapperSetup : IDapperSetup
    {
        private readonly IDbConnection connection;
        private readonly string connectionString;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="connection"></param>
        public DapperSetup(string connectionString, IDbConnection connection = null)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("DataSource string connection error");

            this.connection = connection;
            this.connectionString = connectionString;
        }

        private IDbConnection DbConnection()
        {
            return connection ?? new MySqlConnection(connectionString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dbParams"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAll<T>(string sql, object dbParams, CommandType commandType)
        {
            return await DbConnection().QueryAsync<T>(sql, dbParams, commandType: commandType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dbParams"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async Task<T> Get<T>(string sql, object dbParams, CommandType commandType)
        {
            return await DbConnection().QueryFirstOrDefaultAsync<T>(sql, dbParams, commandType: commandType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dbParams"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async Task<T> Insert<T>(string sql, object dbParams, CommandType commandType)
        {
            T result;
            using IDbConnection db = DbConnection();

            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = await db.QuerySingleAsync<T>(sql, dbParams, commandType: commandType, transaction: tran);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dbParams"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async Task<T> Execute<T>(string sql, object dbParams, CommandType commandType)
        {
            T result;
            using IDbConnection db = DbConnection();

            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = await db.ExecuteScalarAsync<T>(sql, dbParams, commandType: commandType, transaction: tran);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }
    }
}
