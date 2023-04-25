using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using WeChatPaySample.Configuration.Dto;

namespace WeChatPaySample.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : WeChatPaySampleAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
