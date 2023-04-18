namespace WeChatPaySample.Common
{
    public class RefundResult
    {
        public RefundResult()
        {
        }

        public string RefundId { get; set; }
        public string ErrCode { get; set; }
        public string ErrCodeDes { get; set; }
    }
}