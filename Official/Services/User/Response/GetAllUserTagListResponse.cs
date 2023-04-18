using System.Collections.Generic;
using WeChat.Official.Infrastructure.Models;
using Newtonsoft.Json;

namespace WeChat.Official.Services.User.Response
{
    public class GetAllUserTagListResponse : OfficialCommonResponse
    {
        [JsonProperty("tags")]
        public List<UserTagDefinition> Tags { get; set; }
    }
}