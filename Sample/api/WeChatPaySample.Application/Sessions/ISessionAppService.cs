using System.Threading.Tasks;
using Abp.Application.Services;
using WeChatPaySample.Sessions.Dto;

namespace WeChatPaySample.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
