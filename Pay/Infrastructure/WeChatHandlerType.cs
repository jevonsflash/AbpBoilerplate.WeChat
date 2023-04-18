using System;

namespace WeChat.Pay.Infrastructure
{
    [Flags]
    public enum WeChatHandlerType
    {
        Normal = 1,
        Refund = 2
    }
}