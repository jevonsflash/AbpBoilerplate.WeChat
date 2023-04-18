using Newtonsoft.Json;

namespace WeChat.Official.Services.TemplateMessage.Request
{
    public class OfficialRequest
    {
        [JsonProperty("appid")]
        public string AppId { get; set; }

        [JsonProperty("pagepath")]
        public string PagePath { get; set; }
    }
}