using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using WeChat.MiniProgram.Configuration;
using WeChat.Pay.Configuration;
using WeChat.Pay.Infrastructure;
using WeChat.Pay.Infrastructure.Handlers;
using WeChat.Pay.Services.Pay;
using WeChatPaySample.Authorization.Users;
using WeChatPaySample.Common;
using WeChatPaySample.Helper;
using WeChatPaySample.Order.Consts;
using WeChatPaySample.WeChatPay;

namespace WeChatPaySample
{
    public class WeChatPayOrderManager : DomainService
    {
        private readonly IRepository<Order.Order, long> orderRepository;

        private readonly IRepository<UserLogin, long> userLoginRepository;
        private readonly UserManager userManager;
        private readonly IPayConfiguration payConfiguration;
        private readonly IMiniProgramConfiguration configuration;
        private readonly OrdinaryMerchantPayService ordinaryMerchantPayService;
        private readonly SignVerifyHandler signVerifyHandler;

        public string OrderType => "微信支付";

        public WeChatPayOrderManager(
            IRepository<Order.Order, long> orderRepository,
            IRepository<UserLogin, long> userLoginRepository,
            UserManager userManager,
            IPayConfiguration payConfiguration,
            IMiniProgramConfiguration configuration,
            OrdinaryMerchantPayService ordinaryMerchantPayService,
            SignVerifyHandler signVerifyHandler

            )
        {
            this.orderRepository = orderRepository;

            this.userLoginRepository = userLoginRepository;
            this.userManager = userManager;
            this.payConfiguration = payConfiguration;
            this.configuration = configuration;
            this.ordinaryMerchantPayService = ordinaryMerchantPayService;
            this.signVerifyHandler = signVerifyHandler;
        }


        public async Task<PrePayResult> PrePayAsync(Order.Order currentOrder, int? tenantId, string loginProvider)
        {
            var price = currentOrder.Payment.Value;
            var outTradeNo = currentOrder.OrderNumber;
            var userId = currentOrder.UserId;
            var openId = "";

            var query = from userLogin in userLoginRepository.GetAll()
                        join user in userManager.Users on userLogin.UserId equals user.Id
                        where user.Id == userId &&
                        userLogin.LoginProvider == loginProvider &&
                        userLogin.TenantId == tenantId
                        select userLogin;

            var currentUserLogin = query.FirstOrDefault();
            if (currentUserLogin != null)
            {
                openId = currentUserLogin.ProviderKey;
            }
            else
            {
                throw new UserFriendlyException("此账号未绑定第三方支付账号，无法使用第三方支付");

            }

            //https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter3_5_5.shtml

            var appId = configuration.AppId;
            var mchId = payConfiguration.MchId;
            var body = currentOrder.Body;
            var attach = "说明:" + currentOrder.Description;
            int totalFee = (int)Math.Floor(price * 100);
            var tradeType = "JSAPI";
            bool isProfitSharing = false;
            var xmlResult = await ordinaryMerchantPayService.UnifiedOrderAsync(appId, mchId, body, attach, outTradeNo, totalFee, tradeType, openId, isProfitSharing);
            await ValidateXmlData(xmlResult);

            var prePayInfo = XmlSerialize.DeserializeXML<WeChatPayUnifiedOrderResponseModel>(xmlResult.InnerXml);

            if (prePayInfo.ReturnCode != "SUCCESS" || prePayInfo.ReturnMsg != "OK")
            {
                throw new UserFriendlyException("未知原因失败");

            }
            if (prePayInfo.ResultCode == "FAIL" && prePayInfo.ErrCode != null)
            {

                switch (prePayInfo.ErrCode)
                {
                    case "ORDERCLOSED":
                    case "ORDERPAID":
                        await FinishPayAsync(currentOrder);
                        break;

                    default:
                        break;
                }

                throw new UserFriendlyException("下单失败:" + "[" + prePayInfo.ErrCode + "]" + prePayInfo.ErrCodeDes);

            }

            var prepayId = prePayInfo.PrepayId;
            var nonceStr = prePayInfo.NonceStr;

            return new PrePayResult()
            {
                PrepayId = prepayId,
                NonceStr = nonceStr,
            };
        }

        /// <summary>
        /// 前端返回“成功”或“报错”的情况，商户需要调用商户查单接口，确认订单状态
        /// https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter3_5_5.shtml
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<Order.Order> FinishPayAsync(Order.Order order)
        {
            if (order.Status == OrderStatus.已支付 || order.Status == OrderStatus.已关闭)
            {
                return order;
            }

            var appId = configuration.AppId;
            var mchId = payConfiguration.MchId;
            var xmlResult = await ordinaryMerchantPayService.OrderQueryAsync(appId, mchId, orderNo: order.OrderNumber);
            await ValidateXmlData(xmlResult);
            var queryOrderInfo = XmlSerialize.DeserializeXML<WeChatPayQueryOrderResponseModel>(xmlResult.InnerXml);
            if (queryOrderInfo.ReturnCode != "SUCCESS" || queryOrderInfo.ReturnMsg != "OK")
            {
                throw new UserFriendlyException("未知原因失败");

            }
            if (queryOrderInfo.ResultCode == "FAIL" && queryOrderInfo.ErrCode != null)
            {
                throw new UserFriendlyException("交易失败:" + "[" + queryOrderInfo.ErrCode + "]" + queryOrderInfo.ErrCodeDes);
            }

            Order.Order result;

            var wechatOrderId = queryOrderInfo.TransactionId;
            order.TradeNo = wechatOrderId;
            order.Type = OrderType;
            order.EndTime = string.IsNullOrEmpty(queryOrderInfo.TimeEnd) ? default : DateTime.ParseExact(queryOrderInfo.TimeEnd, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            switch (queryOrderInfo.TradeState)
            {
                case "SUCCESS":
                    order.Status = OrderStatus.已支付;
                    break;
                case "NOTPAY":
                case "USERPAYING":
                case "ACCEPT":
                    break;
                case "CLOSED":
                case "REVOKED":
                case "PAYERROR":
                case "REFUND":
                    order.Status = OrderStatus.已关闭;
                    break;
                default:
                    break;
            }

            result = await orderRepository.UpdateAsync(order);
            return result;
        }

        public async Task<Order.Order> CloseAsync(Order.Order order)
        {
            if (order.Status == OrderStatus.已完成 || order.Status == OrderStatus.已退款 || order.Status == OrderStatus.已支付 || order.Status == OrderStatus.已关闭)
            {
                throw new UserFriendlyException("订单已支付或者已关闭，无法操作关闭订单");
            }

            var appId = configuration.AppId;
            var mchId = payConfiguration.MchId;
            var xmlResult = await ordinaryMerchantPayService.CloseOrderAsync(appId, mchId, orderNo: order.OrderNumber);
            await ValidateXmlData(xmlResult);
            var queryOrderInfo = XmlSerialize.DeserializeXML<WeChatPayResponseModelBase>(xmlResult.InnerXml);
            if (queryOrderInfo.ReturnCode != "SUCCESS" || queryOrderInfo.ReturnMsg != "OK")
            {
                throw new UserFriendlyException("未知原因失败");

            }
            if (queryOrderInfo.ResultCode == "FAIL" && queryOrderInfo.ErrCode != null)
            {
                throw new UserFriendlyException("关闭订单失败:" + "[" + queryOrderInfo.ErrCode + "]" + queryOrderInfo.ErrCodeDes);
            }

            order.Status = OrderStatus.已关闭;
            var result = await orderRepository.UpdateAsync(order);
            return result;
        }

        private async Task ValidateXmlData(XmlDocument xmlResult)
        {
            var weChatPayHandlerContext = new WeChatPayHandlerContext()
            {
                WeChatRequestXmlData = xmlResult,

            };
            await signVerifyHandler.HandleAsync(weChatPayHandlerContext);
            if (weChatPayHandlerContext.IsSuccess == false)
            {
                throw new UserFriendlyException(weChatPayHandlerContext.FailedResponse);
            }
        }


    }
}
