#region

using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using PixelPubApi.MySQL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore;
using Serilog;

#endregion

namespace PixelPubApi.Middleware
{
    public class ApiAuthMiddleWare
    {
        private RequestDelegate next;
        public ApiAuthMiddleWare(RequestDelegate _next)
        {
            next = _next;
        }

        public async Task Invoke(HttpContext context, WrathIncarnateContext dbcontext)
        {
            var request  = context.Request;
            var headers  = request.Headers;
            var env      = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevEnv = env == "Local" || env == "Development";

            if(request.Path.ToString().IndexOf("/swagger") == 0) {
                await next.Invoke(context);
            } else {
                var key   = !string.IsNullOrEmpty(headers["X-API-KEY"].ToString()) ? headers["X-API-KEY"].ToString() : "";
                var found = validate(key, dbcontext);

                if (found == false)
                {
                    logFailure(context);
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid API Key");
                }
                else
                {
                    await next.Invoke(context);
                }
            }
        }



        private bool validate(string key, WrathIncarnateContext context)
        {
            var found = context.ApiAccess.FirstOrDefault(a => a.key == key);

            return found != null;
        }
        
        private void logFailure(HttpContext context)
        {
            var headers    = context.Request.Headers;
            var key        = headers["X-API-KEY"].ToString() ?? "N/A";

            var message = $"AUTH FAILURE: {context.Connection.RemoteIpAddress} attempting to access {context.Request.Path}."
                + $" Supplied Credentials: X-API-KEY: {key}";

            Log.Warning(message);
        }
    }
}