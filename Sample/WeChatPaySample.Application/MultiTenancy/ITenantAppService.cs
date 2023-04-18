using Abp.Application.Services;
using WeChatPaySample.MultiTenancy.Dto;

namespace WeChatPaySample.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

