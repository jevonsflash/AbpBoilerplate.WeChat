using WeChat.Official.Infrastructure.Models;
using Newtonsoft.Json;

namespace WeChat.Official.Services.User.Request
{
    public class GetTagsByUserRequest : OfficialCommonRequest
    {
        [JsonProperty("openid")] 
        public string OpenId { get; protected set; }

        public GetTagsByUserRequest(string openId)
        {
            OpenId = openId;
        }
    }
}