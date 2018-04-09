#region

using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;

#endregion

namespace PixelPubApi.Utilities
{
    public class LoggingHandler : DelegatingHandler
    {
        private const string MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        private HttpClientHandler _innerHandler;

        public LoggingHandler(HttpClientHandler handler)
        {
            _innerHandler = handler;
        }

        public LoggingHandler()
        {

        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            if(_innerHandler != null) {
                InnerHandler = _innerHandler;
            }
            
            if(InnerHandler == null) {
                InnerHandler = new HttpClientHandler() { UseCookies = false };
            }
            var start    = Stopwatch.GetTimestamp();
            var response = await base.SendAsync(requestMessage, cancellationToken);
            var elapsed  = (Stopwatch.GetTimestamp() - start) * 1000 / (double)Stopwatch.Frequency;
            var log      = LogForErrorContext(response);
            var level    = elapsed > 3000 ? LogEventLevel.Error : LogEventLevel.Warning;

            log.Write(level, MessageTemplate, requestMessage.Method.Method, requestMessage.RequestUri.ToString(), ((int)response.StatusCode).ToString(), elapsed);
       
            return response;
        }

        private static ILogger LogForErrorContext(HttpResponseMessage response) {
            var request = response.RequestMessage;

            var result = Log
                .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), true)
                .ForContext("RequestHost", request.RequestUri.Host)
                .ForContext("RequestProperties", request.Properties.ToDictionary(h => h.Key, h => h.Value.ToString()), true);

            return result;
        }
    }
}
