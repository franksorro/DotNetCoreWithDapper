using Backend.Middlewares;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [
        ApiController,
        Route("api/[controller]/[action]"),
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