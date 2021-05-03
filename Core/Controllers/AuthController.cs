using Microsoft.AspNetCore.Mvc;
using Core.Middlewares;

namespace Core.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [
        ApiController,
        Route("api/v{version:apiVersion}/[controller]/[action]"),
        Produces("application/json"),
        ServiceFilter(typeof(AuthFilter))
    ]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        public AuthController() { }
    }
}