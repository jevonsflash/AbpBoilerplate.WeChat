using System.Threading.Tasks;
using WeChatPaySample.Configuration.Dto;

namespace WeChatPaySample.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
