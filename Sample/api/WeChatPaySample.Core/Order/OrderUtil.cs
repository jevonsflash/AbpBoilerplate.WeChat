using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Extensions;

namespace WeChatPaySample.Order
{
    //订单号规则
    //'1220614276870050891'
    //订单类型：1.付款订单  2.退款订单
    //1
    //年+月+日
    //220614
    //订单发起者就诊卡号码后5位
    //27687
    //附加信息 01：保留
    //01
    //5位随机数
    //50891

    public class OrderUtil
    {
        public static string GetOrderNumber(string number, string affix = "01")
        {
            var date = DateTime.Now;
            var employeeNumber = $"1{date.Year.ToString().Substring(2)}{date.Month.ToString().PadLeft(2, '0')}{date.Day.ToString().PadLeft(2, '0')}{number}{affix}{RandomHelper.GetRandom(100000).ToString().PadRight(5, '0')}";
            return employeeNumber;
        }
    }
}
