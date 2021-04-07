using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Backend.Extensions;
using Backend.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Backend.Models;

namespace Backend.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    public class HeaderFilter : IAsyncActionFilter
    {
        private readonly AppSettings settings;
        private readonly IClientCacheService clientCacheService;
        private readonly string cacheKey;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientCacheService"></param>
        /// <param name="settings"></param>
        public HeaderFilter(
            IClientCacheService clientCacheService,
            IOptions<AppSettings> settings,
            string cacheKey)
        {
            this.settings = settings.Value;
            this.clientCacheService = clientCacheService;
            this.cacheKey = cacheKey;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(
                ActionExecutingContext context,
                ActionExecutionDelegate next)
        {
            if (settings.ClientCache.Enabled)
            {
                string eTagClient = context.HttpContext.Request.Headers["If-None-Match"].FirstOrDefault();

                if (string.IsNullOrEmpty(eTagClient))
                {
                    context.Result = new BadRequestObjectResult("If-None-Match key not found")
                    {
                        StatusCode = 400
                    };

                    return;
                }

                ClientCacheModel clientCache = await clientCacheService.Get(cacheKey);
                if (clientCache == null)
                {
                    context.Result = new BadRequestObjectResult("Database key not found")
                    {
                        StatusCode = 400
                    };

                    return;
                }

                long eTagServer = clientCache.CacheValue;

                if (eTagServer.ToString() == eTagClient)
                {
                    context.Result = new ObjectResult("Not modified")
                    {
                        StatusCode = 304
                    };

                    return;
                }

                context.HttpContext.Response.Headers.Add("Last-Modified", eTagServer.ToDateTimeFrom1970().ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
                context.HttpContext.Response.Headers.Add("ETag", eTagServer.ToString());
            }
            
            await next();
        }
    }
}