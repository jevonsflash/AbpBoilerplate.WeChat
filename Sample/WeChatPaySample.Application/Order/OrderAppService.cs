using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatPaySample.Common;

namespace WeChatPaySample.Order
{
    public class OrderAppService : AsyncCrudAppService<Order, OrderDto, long>
    {
        public OrderAppService(IRepository<Order, long> repository) : base(repository)
        {
        }


    }
}
