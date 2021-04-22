using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Core.Middlewares;
using Core.Controllers;
using Backend.Services;
using Backend.Types;
using Backend.Models;
using Microsoft.Extensions.Localization;

namespace Backend.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class TestController : BaseController
    {
        private readonly TestService service;
        private readonly ILogger<TestController> logger;
        private readonly IStringLocalizer<TestController> localizer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public TestController(
            TestService service,
            ILogger<TestController> logger,
            IStringLocalizer<TestController> localizer)
        {
            this.service = service;
            this.logger = logger;
            this.localizer = localizer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [
            HttpGet, 
            TypeFilter(typeof(HeaderFilter), Arguments = new object[] { ClientCacheType.Test.GetAll })
        ]
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
        [
            HttpGet("{id}"),
            TypeFilter(typeof(HeaderFilter), Arguments = new object[] { ClientCacheType.Test.GetAll })
        ]
        public async Task<IActionResult> Get(int id)
        {
            TestModel model = await service.Get(id);

            if (model == null)
            {
                logger.LogError($"GetById {id} not found");

                return NotFound(string.Format(localizer["NotFound"].Value, id));
            }

            logger.LogInformation("GetById service called");

            return Ok(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TestModel model)
        {
            return Ok(await service.Add(model));
        }
    }
}
