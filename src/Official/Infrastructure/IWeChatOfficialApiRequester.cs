using System.Net.Http;
using System.Threading.Tasks;
using WeChat.Official.Infrastructure.Models;

namespace WeChat.Official.Infrastructure
{
    public interface IWeChatOfficialApiRequester
    {
        Task<string> RequestAsync(string targetUrl,
            HttpMethod method,
            IOfficialRequest officialRequest = null,
            bool withAccessToken = true);

        Task<TResponse> RequestAsync<TResponse>(string targetUrl,
            HttpMethod method,
            IOfficialRequest officialRequest = null,
            bool withAccessToken = true);

        Task<TResponse> RequestFromDataAsync<TResponse>(string targetUrl,
            MultipartFormDataContent formDataContent,
            IOfficialRequest officialRequest = null,
            bool withAccessToken = true);
    }
}