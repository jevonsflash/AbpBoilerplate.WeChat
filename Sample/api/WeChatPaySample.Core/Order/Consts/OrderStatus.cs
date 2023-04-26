using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatPaySample.Order.Consts
{
    //已下单-已下单
    //已支付-已支付
    //已完成-已完成(正常流转完毕)
    //已关闭-已关闭(未支付的正常流转完毕)
    //已取消-已取消(异常流程)
    //已退款-已退款
    public class OrderStatus
    {
        public const string 已下单 = "已下单";
        public const string 已支付 = "已支付";
        public const string 已关闭 = "已关闭";
        public const string 已退款 = "已退款";

    }

    public class OrderExtensionStatus
    {
        public const string 未完成 = "未完成";
        public const string 已完成 = "已完成";
    }
}
