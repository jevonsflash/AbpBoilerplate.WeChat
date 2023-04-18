using System.Xml.Serialization;

namespace WeChatPaySample.WeChatPay
{
    [XmlRoot(ElementName = "xml", Namespace = "")]
    public class WeChatPayModelBase
    {

        public const string SUCCESS = "SUCCESS";
        public const string FAIL = "FAIL";

        public WeChatPayModelBase()
        {
            ReturnCode = SUCCESS;
            ReturnMsg = "OK";

        }

        /// <summary>
        /// 返回状态码
        /// SUCCESS/FAIL(此字段是通信标识，非交易标识，交易是否成功需要查看trade_state来判断)
        /// </summary>
        [XmlElement("return_code")]
        public string ReturnCode { get; set; }

        /// <summary>
        /// 返回信息，错误原因
        /// </summary>
        [XmlElement("return_msg")]
        public string ReturnMsg { get; set; }
    }
}