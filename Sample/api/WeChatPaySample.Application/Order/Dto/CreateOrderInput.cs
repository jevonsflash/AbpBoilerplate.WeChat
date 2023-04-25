using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;

namespace WeChatPaySample.Order.Dto
{
    [AutoMapTo(typeof(Order))]
    public class CreateOrderInput : EntityDto<long>
    {
        public long UserId { get; set; }
        public string Body { get; set; }

        public string Description { get; set; }

        public decimal? Payment { get; set; }



    }
}