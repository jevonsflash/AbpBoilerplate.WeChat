using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace WeChatPaySample.Controllers
{
    public abstract class WeChatPaySampleControllerBase: AbpController
    {
        protected WeChatPaySampleControllerBase()
        {
            LocalizationSourceName = WeChatPaySampleConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
