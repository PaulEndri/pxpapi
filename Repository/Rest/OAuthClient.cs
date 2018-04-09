#region

using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PixelPubApi.Models.Settings;
using PixelPubApi.Utilities;
using Newtonsoft.Json;

#endregion

namespace PixelPubApi.Repository.Rest
{
    public class OAuthClient : RestRepository
    {
        public OAuthClient(RestSource source) : base(source)
        {
        }

        public async Task<T> authenticate<T>()
        {
            var _client = new HttpClient(new LoggingHandler(), true) {
                BaseAddress = new Uri(_source.Uri)
            };

            var content  = new StringContent(_source.Auth, Encoding.UTF8, _source.ContentType);
            var response = await _client.PostAsync(_source.AuthUri, content);
        
            if (response.IsSuccessStatusCode) {
                using (var responseStream = await response.Content.ReadAsStreamAsync()) {
                    var sr = new StreamReader(responseStream);
                    return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
                }
            }

            throw new Exception(response.ReasonPhrase);
        }
    }
}

