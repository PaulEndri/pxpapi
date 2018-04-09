#region

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PixelPubApi.Interfaces;
using PixelPubApi.Models.Settings;
using PixelPubApi.Utilities;
using Newtonsoft.Json;

#endregion

namespace PixelPubApi.Repository
{
    public abstract class RestRepository : IRestRepository
    {
        protected HttpClient client;
        protected RestSource _source;
        private HttpClientHandler _handler;
        protected Dictionary<string, string> additionalHeaders;

        protected RestRepository(RestSource source) {
            _source  = source;
            _handler = new HttpClientHandler();
        }

        public virtual HttpClient getClient(HttpClientHandler handler = null) {
            var _client = new HttpClient(new LoggingHandler(handler), true)
            {
                BaseAddress = new Uri(_source.Uri)
            };

            return _client;
        }

        /**
         * Remember kids, JSON or Bust - This is a REST API afterall
         **/
        public async Task<T> get<T>(string route)
        {
            if(!string.IsNullOrEmpty(_source.Path)) {
                route = _source.Path + route;
            }

            var _client = getClient(_handler);
            var response = await _client.GetAsync(route);
            var result   = await response.Content.ReadAsStringAsync();
    
            
            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<HttpResponseMessage> post<T>(string route, T postObject) {
            if (!string.IsNullOrEmpty(_source.Path)) {
                route = _source.Path + route;
            }

            var response = await getClient(_handler).PostAsync(route, new StringContent(JsonConvert.SerializeObject(postObject), Encoding.UTF8, "application/json"));
            
            return response;
        }
        public async Task<T> post<T, U>(string route, U postObject) {
            if (!string.IsNullOrEmpty(_source.Path)) {
                route = _source.Path + route;
            }

            var response = await getClient(_handler).PostAsync(route, new StringContent(JsonConvert.SerializeObject(postObject), Encoding.UTF8, "application/json"));
            var result   = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }

        public virtual void injectExtraHeaders(HttpClient client)
        {
            if (additionalHeaders.Count > 0) {
                foreach (var extraHeader in additionalHeaders) {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(extraHeader.Key, extraHeader.Value);
                }
            }
        }

        public virtual void registerHandler(HttpClientHandler handler)
        {
            _handler = handler;
        }
    }
}
