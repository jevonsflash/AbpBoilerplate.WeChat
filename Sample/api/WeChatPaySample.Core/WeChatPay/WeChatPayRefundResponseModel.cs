using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WeChatPaySample.WeChatPay
{
    [XmlRoot("xml")]
    public class WeChatPayRefundResponseModel : WeChatPayResponseModelBase
    {
        /// <summary>
        /// 微信订单号
        /// </summary>
        [XmlElement("transaction_id")]
        public string TransactionId { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        [XmlElement("out_trade_no")]
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 商户退款单号
        /// </summary>
        [XmlElement("out_refund_no_0")]
        public string OutRefundNo { get; set; }

        /// <summary>
        /// 微信退款单号
        /// </summary>
        [XmlElement("refund_id_0")]
        public string RefundId { get; set; }

        /// <summary>
        /// 退款渠道 ORIGINAL—原路退款 BALANCE—退回到余额
        /// </summary>
        [XmlElement("refund_channel_0")]
        public string RefundChannel { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        [XmlElement("refund_fee_0")]
        public int RefundFee { get; set; }

        /// <summary>
        /// 退款货币种类
        /// </summary>
        [XmlIgnore]
        [Obsolete]
        public string RefundFeeType { get; set; }

        /// <summary>
        /// 订单总金额
        /// </summary>
        [XmlElement("total_fee")]
        public int TotalFee { get; set; }

        /// <summary>
        /// 订单金额货币种类
        /// </summary>
        [XmlElement("fee_type")]
        public string FeeType { get; set; }

        /// <summary>
        /// 现金支付金额
        /// </summary>
        [XmlElement("cash_fee")]
        public int CashFee { get; set; }

        /// <summary>
        /// 货币种类
        /// </summary>
        [XmlIgnore]
        [Obsolete]
        public string CashFeeType { get; set; }

        /// <summary>
        /// 现金退款金额
        /// </summary>
        [XmlElement("cash_refund_fee")]
        public int CashRefundFee { get; set; }

        /// <summary>
        /// 现金退款货币类型
        /// </summary>
        [XmlIgnore]
        [Obsolete]
        public string CashRefundFeeType { get; set; }

        /// <summary>
        /// 代金券或立减优惠退款金额
        /// </summary>
        [XmlElement("coupon_refund_fee")]
        public int CouponRefundFee { get; set; }


        /// <summary>
        /// 代金券或立减优惠使用数量
        /// </summary>
        [XmlElement("coupon_count")]
        public int CouponCount { get; set; }

        /// <summary>
        /// 代金券或立减优惠ID
        /// </summary>
        [XmlElement("coupon_refund_id")]
        public string CouponRefundId { get; set; }

        /// <summary>
        /// 退款状态
        /// </summary>
        [XmlElement("refund_status_0")]
        public string RefundStatus { get; set; }
    }
}
