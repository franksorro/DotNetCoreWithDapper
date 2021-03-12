using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Backend.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthFilter : ActionFilterAttribute, IAsyncActionFilter
    {
        private readonly AppSettings settings;

        /// <summary>
        /// 
        /// </summary>
        public AuthFilter(
            IOptions<AppSettings> settings)
        {
            this.settings = settings.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override async Task OnActionExecutionAsync(
                ActionExecutingContext context,
                ActionExecutionDelegate next)
        {
            string authHeader = context.HttpContext.Request.Headers[settings.Authorization.ApiKeyName].FirstOrDefault();

            if (string.IsNullOrEmpty(authHeader) || (!string.IsNullOrEmpty(authHeader) && !authHeader.Equals(settings.Authorization.ApiKeyValue)))
            {
                context.Result = new ObjectResult("Not authorized.")
                {
                    StatusCode = 401
                };

                return;
            }

            await next();
        }
    }
}