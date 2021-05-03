using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Core.Middlewares;
using Core.Controllers;
using Backend.Services;
using Backend.Types;

namespace Backend.Controllers.V2
{
    /// <summary>
    /// 
    /// </summary>
    [
        ApiVersion("2.0"),
        ApiExplorerSettings(GroupName = "v2")
    ]
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
        [
            HttpGet,
            TypeFilter(typeof(HeaderFilter), Arguments = new object[] { ClientCacheType.Test.GetAll })
        ]
        public async Task<IActionResult> Get()
        {
            logger.LogInformation("Get service v2 called");

            return Ok(await service.GetAll());
        }
    }
}
