using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Core.Controllers;
using Backend.Models;
using Microsoft.Extensions.Localization;
using FS.Interfaces;

namespace Backend.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class DynamoDBController : BaseController
    {
        private readonly IDynamoDBService service;
        private readonly ILogger<TestController> logger;
        private readonly IStringLocalizer<TestController> localizer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public DynamoDBController(
            IDynamoDBService service,
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
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            logger.LogInformation("DynamoDB get service called");
            return Ok(await service.SelectAsync<DynamoTestModel>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            DynamoTestModel model = await service.SelectSingleAsync<DynamoTestModel>(new object[] { id });

            if (model == null)
            {
                logger.LogError($"DynamoDB getById {id} not found");

                return NotFound(string.Format(localizer["NotFound"].Value, id));
            }

            logger.LogInformation("DynamoDB getById service called");

            return Ok(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddOrEdit([FromBody] DynamoTestModel model)
        {
            await service.InsertOrUpdateAsync(model);
            return Ok();
        }
    }
}