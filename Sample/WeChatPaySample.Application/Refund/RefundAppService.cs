using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatPaySample.Common;

namespace WeChatPaySample.Refund
{
    public class RefundAppService : ApplicationService
    {
        private readonly RefundManager refundManager;

        public RefundAppService(RefundManager refundManager)
        {
            this.refundManager = refundManager;
        }
        public async Task<RefundResult> RefundAsync(RefundModel refundCorrelation)
        {
            var result = await refundManager.RefundAsync(refundCorrelation);
            return result;
        }

        public async Task<FinishRefundResult> FinishRefundAsync(string refundNumber)
        {
            var result = await refundManager.FinishRefundAsync(refundNumber);
            return result;
        }

    }
}
