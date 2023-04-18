using Abp.Application.Services;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatPaySample.Common;

namespace WeChatPaySample.Pay
{
    public class PayAppService : ApplicationService
    {
        private readonly WeChatPayOrderManager orderManager;
        private readonly IRepository<Order.Order, long> orderRepository;

        public PayAppService(WeChatPayOrderManager orderManager,IRepository<Order.Order,long> orderRepository)
        {
            this.orderManager = orderManager;
            this.orderRepository = orderRepository;
        }
        public async Task<PrePayResult> PrePay(PrePayInput n)
        {
            var currentOrder = orderRepository.Get(n.Id);
            if (!currentOrder.Payment.HasValue)
            {
                throw new UserFriendlyException("此订单未指定金额");

            }
            var result = await orderManager.PrePayAsync(currentOrder, AbpSession.TenantId, n.LoginProvider);
            return result;
        }

        public async Task<Order.Order> FinishPayAsync(Order.Order order)
        {
            var result = await orderManager.FinishPayAsync(order);
            return result;
        }


        public async Task<Order.Order> CloseAsync(Order.Order order)
        {
            var result = await orderManager.CloseAsync(order);
            return result;
        }
    }
}
