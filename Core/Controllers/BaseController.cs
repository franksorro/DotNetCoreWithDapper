using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [
        ApiController,
        Route("api/v{version:apiVersion}/[controller]/[action]"),
        Produces("application/json")
    ]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        public BaseController() { }
    }
}