using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Linq;
using Abp.Dependency;
using WeChat.Common;
using WeChat.Common.Infrastructure.AccessToken;
using WeChat.Official.Configuration;

namespace WeChat.Official.Infrastructure
{
    public class DefaultAccessTokenAccessor : IAccessTokenAccessor, ISingletonDependency
    {
        private readonly IOfficialConfiguration options;

        private readonly IAccessTokenProvider _accessTokenProvider;

        public DefaultAccessTokenAccessor(
            IAccessTokenProvider accessTokenProvider,
            IOfficialConfiguration options)
        {
            _accessTokenProvider = accessTokenProvider;
            this.options = options;
        }

        public virtual async Task<string> GetAccessTokenAsync()
        {
            return await _accessTokenProvider.GetAccessTokenAsync(options.AppId, options.AppSecret);
        }
    }
}