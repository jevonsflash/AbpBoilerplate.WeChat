using System.Threading.Tasks;

namespace WeChat.Pay.Infrastructure
{
    /// <summary>
    /// 定义了微信支付回调处理器。
    /// </summary>
    public interface IWeChatPayHandler
    {
        Task HandleAsync(WeChatPayHandlerContext context);

        WeChatHandlerType Type { get; }
    }
}