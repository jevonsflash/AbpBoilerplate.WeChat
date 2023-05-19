using Newtonsoft.Json;
using WeChat.MiniProgram.Models;

namespace WeChat.MiniProgram.Services.PhoneNumber
{
    public class GetPhoneNumberResponse : IMiniProgramResponse
    {
        public string ErrorMessage { get; set; }

        public int ErrorCode { get; set; }

        [JsonProperty("phone_info")]
        public PhoneInfo PhoneInfo { get; set; }
    }


    public class PhoneInfo
    {
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty("purePhoneNumber")]
        public string PurePhoneNumber { get; set; }
        [JsonProperty("countryCode")]
        public int CountryCode { get; set; }
        [JsonProperty("watermark")]
        public Watermark Watermark { get; set; }
    }

    public class Watermark
    {
        [JsonProperty("timestamp")]
        public int Timestamp { get; set; }
        [JsonProperty("appid")]
        public string Appid { get; set; }
    }
}