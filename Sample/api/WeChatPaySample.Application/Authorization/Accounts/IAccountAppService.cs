using System.Threading.Tasks;
using Abp.Application.Services;
using WeChatPaySample.Authorization.Accounts.Dto;

namespace WeChatPaySample.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
