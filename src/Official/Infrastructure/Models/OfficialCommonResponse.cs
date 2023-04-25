using Newtonsoft.Json;

namespace WeChat.Official.Infrastructure.Models
{
    public class OfficialCommonResponse : IOfficialResponse
    {
        [JsonProperty("errmsg")] 
        public string ErrorMessage { get; set; }

        [JsonProperty("errcode")] 
        public int ErrorCode { get; set; }
    }
}