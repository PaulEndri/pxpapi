#region

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using PixelPubApi.Models.Settings;
using PixelPubApi.Utilities;

#endregion

namespace PixelPubApi.Repository.Rest
{
    public class BaseClient : RestRepository
    {
        public BaseClient(RestSource source) : base(source)
        {
            additionalHeaders = new Dictionary<string, string>();
        }

        public override HttpClient getClient(HttpClientHandler handler = null)
        {
            return authenticate(handler);
        }

        private HttpClient authenticate(HttpClientHandler handler)
        {
            HttpClient _client = new HttpClient(new LoggingHandler()) {
                BaseAddress = new Uri(_source.Uri)
            };

            if(handler != null) {
                _client = new HttpClient(handler) {
                    BaseAddress = new Uri(_source.Uri)
                };
           }


            if(string.IsNullOrWhiteSpace(_source.Auth)) {
                return _client;
            }

            var authStr = "Authorization";
            var authCtx = _source.Auth;

            if(_source.AuthHeader != null) {
                authStr = _source.AuthHeader;
                _client.DefaultRequestHeaders.TryAddWithoutValidation(authStr, authCtx);
            } else {
                var bytes = Encoding.UTF8.GetBytes(authCtx);
                authCtx   = "Basic " + Convert.ToBase64String(bytes);
                _client.DefaultRequestHeaders.Add(authStr, authCtx);

            }

            injectExtraHeaders(_client);

            _client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", _source.ContentType ?? "application/json");

            return _client;
        }
    }
}

