#region

using System.Net.Http;
using System.Threading.Tasks;

#endregion

namespace PixelPubApi.Interfaces
{
    public interface IRestRepository
    {
        HttpClient getClient(HttpClientHandler handler = null);
        Task<T> get<T>(string route);
        Task<HttpResponseMessage> post<T>(string route, T postObject);
        Task<T> post<T, U>(string route, U postObject);
        void injectExtraHeaders(HttpClient client);
        void registerHandler(HttpClientHandler handler);
    }
}
