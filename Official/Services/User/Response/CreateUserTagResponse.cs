using WeChat.Official.Infrastructure.Models;
using Newtonsoft.Json;

namespace WeChat.Official.Services.User.Response
{
    public class CreateUserTagResponse : OfficialCommonResponse
    {
        /// <summary>
        /// 创建成功的标签。
        /// </summary>
        [JsonProperty("tag")]
        public UserTagDefinition Tag { get; set; }
    }
}