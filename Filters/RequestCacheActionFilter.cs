#region

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

#endregion

namespace PixelPubApi.Filters
{
    public class RequestCacheActionFilter : IAsyncActionFilter
    {
        private IMemoryCache _cache;
        public RequestCacheActionFilter(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task OnActionExecutionAsync( ActionExecutingContext actionContext, ActionExecutionDelegate next)
        {
            var context = actionContext.HttpContext;

            if (context.Request.Method.ToUpper() != "GET") {
                await next();
            } else {
                var requestPath = context.Request.Path;
                var cachedRequest = _cache.Get("cachedRequest" + requestPath);

                if (cachedRequest != null) {
                    actionContext.Result = (IActionResult)cachedRequest;
                } else {
                    await next();

                    if(context.Response.StatusCode == 200) {
                        _cache.Set("cachedRequest" + requestPath, actionContext.Result);
                    }
                }
            }
        }
    }
}