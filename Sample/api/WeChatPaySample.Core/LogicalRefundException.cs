using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp;

namespace WeChatPaySample
{



    public class LogicalRefundException : AbpException
    {
        public LogicalRefundException(string errCode, string errCodeDes) : base("退款申请失败:" + "[" + errCode + "]" + errCodeDes)
        {
            ErrCode = errCode;
            ErrCodeDes = errCodeDes;
        }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrCode { get; set; }

        /// <summary>
        /// 错误代码描述
        /// </summary>

        public string ErrCodeDes { get; set; }
    }
}
