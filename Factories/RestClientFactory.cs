#region

using System;
using System.Collections.Generic;
using PixelPubApi.Interfaces;
using PixelPubApi.Models.Settings;
using PixelPubApi.Repository.Rest;
using Microsoft.Extensions.Options;

#endregion

namespace PixelPubApi.Factory
{
    public class RestClientFactory :IRestClientFactory
    {
        private List<RestSource> sources;
        public RestClientFactory(IOptions<List<RestSource>> _sources)
        {
            sources = _sources.Value;
        }

        public IRestRepository get(string name)
        {
            var found = sources.Find(s => s.Name == name);

            if(found == null) {
                throw new Exception("Invalid API Source specified");
            }

            if(found.Type == "OAuth") {
                return new OAuthClient(found);
            }
            else {
                return new BaseClient(found);
            }
        }
    }
}
