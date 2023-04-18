using WeChat.Official.Infrastructure.Models;
using Newtonsoft.Json;

namespace WeChat.Official.Services.User.Request
{
    public abstract class OperationUserTagRequest : OfficialCommonRequest
    {
        [JsonProperty("tag")] 
        public UserTagDefinition Tag { get; protected set; }
    }
}