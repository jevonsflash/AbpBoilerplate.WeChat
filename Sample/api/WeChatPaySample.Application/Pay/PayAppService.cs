using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Logging;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatPaySample.Authorization;
using WeChatPaySample.Common;
using WeChatPaySample.Common.Consts;
using WeChatPaySample.Order;
using WeChatPaySample.Order.Consts;
using WeChatPaySample.Pay.Dto;

namespace WeChatPaySample.Pay
{
    public class PayAppService : ApplicationService
    {
        private readonly WeChatPayOrderManager orderManager;
        private readonly RefundManager refundManager;
        private readonly IRepository<Order.Order, long> orderRepository;

        public PayAppService(WeChatPayOrderManager orderManager, RefundManager refundManager, IRepository<Order.Order, long> orderRepository)
        {
            this.orderManager = orderManager;
            this.refundManager = refundManager;
            this.orderRepository = orderRepository;
        }
        public async Task<PrePayResult> PrePay(PrePayInput n)
        {
            var currentOrder = orderRepository.Get(n.Id);
            if (!currentOrder.Payment.HasValue)
            {
                throw new UserFriendlyException("此订单未指定金额");

            }
            var result = await orderManager.PrePayAsync(currentOrder, AbpSession.TenantId, n.OpenId);
            return result;
        }

        public async Task<Order.Order> FinishPay(OperationInput n)
        {
            var currentOrder = orderRepository.Get(n.Id);
            if (currentOrder == null)
            {
                throw new UserFriendlyException("找不到订单");
            }

            var order = await orderManager.FinishPayAsync(currentOrder);
            return order;
        }


        public async Task<Order.Order> Close(OperationInput n)
        {
            var currentOrder = orderRepository.Get(n.Id);
            if (currentOrder == null)
            {
                throw new UserFriendlyException("找不到订单");
            }
            var order = await orderManager.CloseAsync(currentOrder);
            return order;
        }

        public async Task<Order.Order> Refund(OperationInput n)
        {

            var currentOrder = orderRepository.Get(n.Id);
            if (currentOrder == null)
            {
                throw new UserFriendlyException("找不到订单");
            }
            if (!currentOrder.Payment.HasValue)
            {
                throw new UserFriendlyException("订单金额不正确");
            }
            if (currentOrder.Payment.Value > 0)
            {

                var refundCorrelation = new RefundModel()
                {
                    Payment = currentOrder.Payment,
                    Description = "全额退款",
                    RefundNumber = currentOrder.OrderNumber,
                    TotalPayment = currentOrder.Payment,
                    Order = currentOrder,
                      
                };

                await refundManager.RefundAsync(refundCorrelation);
            }
            return currentOrder;
        }


    }
}
