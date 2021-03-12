using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class TestRepository : ITestRepository
    {
        private readonly IDapperAsyncService service;

        /// <summary>
        /// 
        /// </summary>
        public TestRepository(IDapperAsyncService service)
        {
            this.service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TestModel>> GetAll()
        {
            return await service.GetsAsync<TestModel>(SQL_Select);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TestModel> Get(int id)
        {
            return await service.GetAsync<TestModel>($"{SQL_Select} WHERE id = @id", new
            {
                id
            });
        }

        #region Queries
        private readonly string SQL_Select = @"
        SELECT
            id,
            description
        FROM test_table";
        #endregion
    }
}
