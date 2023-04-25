using System.Collections.Generic;
using WeChat.Official.Infrastructure.Models;
using Newtonsoft.Json;

namespace WeChat.Official.Services.User.Response
{
    public class GetTagsByUserResponse : OfficialCommonResponse
    {
        [JsonProperty("tagid_list")]
        public List<string> TagIds { get; set; }
    }
}