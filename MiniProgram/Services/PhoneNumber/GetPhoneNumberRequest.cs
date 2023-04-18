using Newtonsoft.Json;
using WeChat.MiniProgram.Models;

namespace WeChat.MiniProgram.Services.ACode
{
    public class GetPhoneNumberRequest : MiniProgramCommonRequest
    {
        [JsonProperty("code")]
        public string Code { get; protected set; }

        public GetPhoneNumberRequest(string code)
        {
            Code = code;
        }


    }
}