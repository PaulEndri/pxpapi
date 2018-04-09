#region

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Serilog.Events;

#endregion

namespace PixelPubApi.Middleware
{
    public class LoggingMiddleware
    {
        private RequestDelegate next;
        private const string MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        private static readonly ILogger Log = Serilog.Log.ForContext<LoggingMiddleware>();

        public LoggingMiddleware(RequestDelegate _next)
        {
            next = _next;
        }

        public async Task Invoke(HttpContext context, IMemoryCache cache)
        {
            var start       = Stopwatch.GetTimestamp();

            try {
                await next.Invoke(context);

                var elapsed = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());
                var status  = (int)context.Response.StatusCode;
                var level   = status > 499 ? LogEventLevel.Error : LogEventLevel.Warning;

                var log = status > 499 ? LogForErrorContext(context) : Log;

                log.Write(level, MessageTemplate, context.Request.Method, context.Request.Path, status.ToString(), elapsed);
            } catch (Exception ex) {
                LogException(context, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()), ex);

                throw ex;
            }
        }

        private static void LogException(HttpContext httpContext, double elapsedMs, Exception ex) {
            LogForErrorContext(httpContext)
                .Error(ex, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, "500", elapsedMs);
        }

        private static ILogger LogForErrorContext(HttpContext httpContext) {
            var request = httpContext.Request;

            var result = Log
                .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), true)
                .ForContext("RequestHost", request.Host)
                .ForContext("RequestProtocol", request.Protocol);

            if (request.HasFormContentType)
                result = result.ForContext("RequestForm", request.Form.ToDictionary(v => v.Key, v => v.Value.ToString()));

            return result;
        }

        private static double GetElapsedMilliseconds(long start, long stop) {
            return (stop - start) * 1000 / (double)Stopwatch.Frequency;
        }
    }
}