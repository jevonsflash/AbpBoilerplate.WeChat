using Abp.Application.Services.Dto;

namespace WeChatPaySample.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

