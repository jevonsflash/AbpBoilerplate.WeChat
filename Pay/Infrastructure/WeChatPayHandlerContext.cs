using System.Xml;

namespace WeChat.Pay.Infrastructure
{
    public class WeChatPayHandlerContext
    {
        public XmlDocument WeChatRequestXmlData { get; set; }

        public bool IsSuccess { get; set; } = true;

        public string FailedResponse { get; set; } = null;
    }
}