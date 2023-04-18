using Abp.Domain.Repositories;
using Abp.Domain.Services;
using WeChat.Pay.Configuration;
using WeChat.MiniProgram.Configuration;
using WeChat.Pay.Services.Pay;
using WeChat.Pay.Infrastructure.Handlers;
using System.Xml;
using Abp.UI;
using WeChat.Pay.Infrastructure;
using Abp;
using WeChatPaySample.WeChatPay;
using WeChatPaySample.Order.Consts;
using WeChatPaySample.Common;
using System.Threading.Tasks;
using System;
using WeChatPaySample.Helper;
using WeChatPaySample.Common.Consts;

namespace WeChatPaySample
{
    public class RefundManager : IDomainService
    {
        private readonly IPayConfiguration payConfiguration;
        private readonly IMiniProgramConfiguration configuration;
        private readonly OrdinaryMerchantPayService ordinaryMerchantPayService;
        private readonly SignVerifyHandler signVerifyHandler;

        public RefundManager(
            IPayConfiguration payConfiguration,
            IMiniProgramConfiguration configuration,
            OrdinaryMerchantPayService ordinaryMerchantPayService,
            SignVerifyHandler signVerifyHandler)
        {
            this.payConfiguration = payConfiguration;
            this.configuration = configuration;
            this.ordinaryMerchantPayService = ordinaryMerchantPayService;
            this.signVerifyHandler = signVerifyHandler;
        }


        public async Task<RefundResult> RefundAsync(RefundModel refundCorrelation)
        {
            var currentOrder = refundCorrelation.Order;
            var outTradeNo = currentOrder.OrderNumber;
            var outRefundNo = refundCorrelation.RefundNumber;

            var appId = configuration.AppId;
            var mchId = payConfiguration.MchId;

            var attach = "退款订单:" + currentOrder.Description + ";退款理由:" + refundCorrelation.Description ;
            int refundFee = (int)Math.Floor(refundCorrelation.Payment.Value * 100);
            int totalFee = (int)Math.Floor(refundCorrelation.TotalPayment.Value * 100);

            var xmlResult = await ordinaryMerchantPayService.RefundAsync(appId, mchId, outTradeNo, outRefundNo, totalFee, refundFee, attach);
            await ValidateXmlData(xmlResult);

            var refundInfo = XmlSerialize.DeserializeXML<WeChatPayRefundResponseModel>(xmlResult.InnerXml);

            if (refundInfo.ReturnCode != "SUCCESS" || refundInfo.ReturnMsg != "OK")
            {
                throw new AbpException("未知原因失败");

            }
            if (string.IsNullOrEmpty(refundInfo.RefundId) && refundInfo.ResultCode == "FAIL" && refundInfo.ErrCode != null)
            {
                switch (refundInfo.ErrCode)
                {
                    case "INVALID_REQUEST":
                    case "TRADE_OVERDUE":
                    case "USER_ACCOUNT_ABNORMAL":
                    case "REFUND_FEE_MISMATCH":
                    case "INVALID_TRANSACTIONID":
                    case "PARAM_ERROR":
                    case "APPID_NOT_EXIST":
                    case "MCHID_NOT_EXIST":
                    case "REQUIRE_POST_METHOD":
                    case "SIGNERROR":
                    case "XML_FORMAT_ERROR":
                        throw new LogicalRefundException(refundInfo.ErrCode, refundInfo.ErrCodeDes);
                        break;
                    default:
                        break;
                }


            }


            var refundId = refundInfo.RefundId;


            return new RefundResult()
            {
                RefundId = refundId,
                ErrCode = refundInfo.ErrCode,
                ErrCodeDes = refundInfo.ErrCodeDes
            };
        }


        public async Task<FinishRefundResult> FinishRefundAsync(string refundNumber)
        {
            var appId = configuration.AppId;
            var mchId = payConfiguration.MchId;
            var xmlResult = await ordinaryMerchantPayService.RefundQueryAsync(appId, mchId, refundNo: refundNumber);
            await ValidateXmlData(xmlResult);
            var queryRefunInfo = XmlSerialize.DeserializeXML<WeChatPayQueryRefundResponseModel>(xmlResult.InnerXml);
            if (queryRefunInfo.ReturnCode != "SUCCESS" || queryRefunInfo.ReturnMsg != "OK")
            {
                throw new UserFriendlyException("未知原因失败");

            }
            if (queryRefunInfo.ResultCode == "FAIL" && queryRefunInfo.ErrCode != null)
            {
                throw new UserFriendlyException("退款失败:" + "[" + queryRefunInfo.ErrCode + "]" + queryRefunInfo.ErrCodeDes);
            }

            RefundModel result;

            var wechatRefundId = queryRefunInfo.TransactionId;

            var refund = new FinishRefundResult();

            refund.TradeNo = wechatRefundId;

            switch (queryRefunInfo.RefundStatus)
            {
                case "SUCCESS":
                    refund.Status = RefundStatus.已完成;
                    break;
                case "PROCESSING":
                    break;
                case "CHANGE":
                    refund.Status = RefundStatus.已取消;
                    break;

                case "REFUNDCLOSE":
                    refund.Status = RefundStatus.已关闭;
                    break;
                default:
                    break;
            }
            return refund;
        }

        public async Task ValidateXmlData(XmlDocument xmlResult)
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
