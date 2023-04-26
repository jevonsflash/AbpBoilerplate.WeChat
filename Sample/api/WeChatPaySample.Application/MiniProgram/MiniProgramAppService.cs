using Abp.Application.Services;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Logging;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChat.MiniProgram.Services.Login;
using WeChatPaySample.Common;

namespace WeChatPaySample.Pay
{
    public class MiniProgramAppService : ApplicationService
    {
        private readonly LoginService loginService;

        public MiniProgramAppService(
            LoginService loginService
            )
        {          
            this.loginService = loginService;
        }
        public async Task<Code2SessionResponse> Login(LoginInput input)
        {

            var weChatLoginResult = await loginService.Code2SessionAsync(input.AccessCode);
            if (weChatLoginResult.ErrorCode != 0)
            {
                throw new UserFriendlyException("小程序调用获取token接口失败，原因" + weChatLoginResult.ErrorMessage);
            }
            //小程序调用获取token接口 https://api.weixin.qq.com/cgi-bin/token 返回的token值无法用于网页授权接口！
            //tips：https://www.cnblogs.com/remon/p/6420418.html
            //var userInfo = await loginService.GetUserInfoAsync(weChatLoginResult.OpenId);
            return weChatLoginResult;
        }   
    }
}
