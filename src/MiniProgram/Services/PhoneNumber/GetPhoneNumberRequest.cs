using Newtonsoft.Json;
using WeChat.MiniProgram.Models;

namespace WeChat.MiniProgram.Services.PhoneNumber
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