using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class TestService
    {
        private readonly ITestRepository repo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public TestService(ITestRepository repo)
        {
            this.repo = repo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TestModel>> GetAll()
        {
            return await repo.GetAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TestModel> Get(int id)
        {
            return await repo.Get(id);
        }
    }
}
