using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Interfaces;
using Backend.Models;
using FS.Interfaces;

namespace Backend.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class TestRepository : ITestRepository
    {
        private readonly IDapperService dapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public TestRepository(IDapperService dapper)
        {
            this.dapper = dapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TestModel>> GetAll()
        {
            return await dapper.SelectAsync<TestModel>(SQL_Select);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TestModel> Get(int id)
        {
            return await dapper.SelectSingleAsync<TestModel>($"{SQL_Select} WHERE id = @id", new
            {
                id
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> Add(TestModel model)
        {
            return await dapper.ExecuteAsync<int>(SQL_Insert, new
            {
                model.Description
            }) > 0;
        }

        #region Queries
        private readonly string SQL_Select = @"
        SELECT
            id,
            description
        FROM test_table";

        private readonly string SQL_Insert = @"
        INSERT INTO test_table
        (
            description
        ) VALUES (
            @description
        );

        SELECT LAST_INSERT_ID()";
        #endregion
    }
}
