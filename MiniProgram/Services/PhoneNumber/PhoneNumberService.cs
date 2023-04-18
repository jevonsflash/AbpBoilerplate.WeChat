using System.Net.Http;
using System.Threading.Tasks;
using WeChat.MiniProgram.Services;

namespace WeChat.MiniProgram.Services.ACode
{
    /// <summary>
    /// 手机号服务。
    /// </summary>
    public class PhoneNumberService : CommonService
    {
        public Task<GetPhoneNumberResponse> GetPhoneNumberAsync(string code)
        {
            const string targetUrl = "https://api.weixin.qq.com/wxa/business/getuserphonenumber";

            var request = new GetPhoneNumberRequest(code);

            return WeChatMiniProgramApiRequester.RequestAsync<GetPhoneNumberResponse>(targetUrl,
                HttpMethod.Post, request);
        }
    }
}