using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatPaySample.Common;
using WeChatPaySample.Order.Consts;
using WeChatPaySample.Order.Dto;

namespace WeChatPaySample.Order
{
    public class OrderAppService : AsyncCrudAppService<Order, OrderDto, long, PagedAndSortedResultRequestDto,CreateOrderInput>
    {
        public OrderAppService(IRepository<Order, long> repository) : base(repository)
        {
        }

        public override async Task<OrderDto> CreateAsync(CreateOrderInput input)
        {
            CheckCreatePermission();

            

            var entity = MapToEntity(input);
            entity.Status = OrderStatus.已下单;
            entity.OrderNumber = OrderUtil.GetOrderNumber(input.UserId.ToString().PadLeft(5, '0'));
            entity.Type = "微信支付";

            await Repository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

    }
}
