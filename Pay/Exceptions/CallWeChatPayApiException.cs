using System;
using System.Runtime.Serialization;
using Abp;

namespace WeChat.Pay.Exceptions
{
    [Serializable]
    public class CallWeChatPayApiException : AbpException, IHasErrorCode
    {
        public int Code { get; set; }

        public string Details { get; set; }

        public CallWeChatPayApiException()
        {
        }

        public CallWeChatPayApiException(SerializationInfo serializationInfo,
            StreamingContext context) : base(serializationInfo,
            context)
        {
        }

        public CallWeChatPayApiException(string message) : base(message)
        {
        }
    }
}