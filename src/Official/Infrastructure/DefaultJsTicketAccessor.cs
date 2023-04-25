using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Linq;
using Abp.Dependency;
using WeChat.Common;
using WeChat.Common.Cache;

namespace WeChat.Official.Infrastructure
{
    public class DefaultJsTicketAccessor : IJsTicketAccessor, ISingletonDependency
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAccessTokenAccessor _accessTokenAccessor;
        private readonly AccessTokenCache _distributedCache;

        public DefaultJsTicketAccessor(IHttpClientFactory httpClientFactory,
            IAccessTokenAccessor accessTokenAccessor,
            AccessTokenCache distributedCache)
        {
            _httpClientFactory = httpClientFactory;
            _accessTokenAccessor = accessTokenAccessor;
            _distributedCache = distributedCache;
        }

        public async Task<string> GetTicketJsonAsync()
        {
            var accessToken = await _accessTokenAccessor.GetAccessTokenAsync();

            var key = "CurrentJsTicket";
            var absoluteExpirationRelativeToNow = TimeSpan.FromMinutes(115);
            var absoluteExpiration = DateTime.Now + absoluteExpirationRelativeToNow;


            return await _distributedCache.GetAsync(key,
                  key => _GetAccessTokenAsync(accessToken)
               ,
               absoluteExpireTime: absoluteExpiration);        
        }

        private string _GetAccessTokenAsync(string accessToken)
        {
            var client = _httpClientFactory.CreateClient();
            var requestUrl = $"https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={accessToken}&type=jsapi";

            var result = client.Send(new HttpRequestMessage(HttpMethod.Get, requestUrl))
                .Content.ReadAsStringAsync().Result;
            return result;
        }

        public async Task<string> GetTicketAsync()
        {
            var json = await GetTicketJsonAsync();
            var jObj = JObject.Parse(json);

            return jObj.SelectToken("$.ticket").Value<string>();
        }
    }
}