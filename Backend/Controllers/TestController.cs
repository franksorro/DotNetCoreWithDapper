using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class TestController : BaseController
    {
        private readonly TestService service;
        private readonly ILogger<TestController> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public TestController(
            TestService service,
            ILogger<TestController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            logger.LogInformation("Get service called");

            return Ok(await service.GetAll());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            TestModel model = await service.Get(id);

            if (model == null)
            {
                logger.LogError($"GetById {id} not found");

                return NotFound("Description not found");
            }


            logger.LogInformation("GetById service called");

            return Ok(model);
        }
    }
}
