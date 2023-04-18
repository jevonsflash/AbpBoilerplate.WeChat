using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml;
using Abp.Dependency;
using Microsoft.Extensions.Logging;
using WeChat.Common.Infrastructure;
using WeChat.Common.Infrastructure.Signature;
using WeChat.Pay.Configuration;

namespace WeChat.Pay.Infrastructure.Handlers
{
    /// <summary>
    /// 签名验证处理器，用于验证微信支付回调结果是否合法。
    /// </summary>
    public class SignVerifyHandler : IWeChatPayHandler, ITransientDependency
    {
        private readonly IPayConfiguration options;
        protected readonly ISignatureGenerator SignatureGenerator;
        protected readonly ILogger<SignVerifyHandler> Logger;

        public SignVerifyHandler(IPayConfiguration options,
            ISignatureGenerator signatureGenerator,
            ILogger<SignVerifyHandler> logger)
        {
            this.options = options;
            SignatureGenerator = signatureGenerator;
            Logger = logger;
        }

        public WeChatHandlerType Type => WeChatHandlerType.Normal;

        public async Task HandleAsync(WeChatPayHandlerContext context)
        {
            var parameters = new WeChatParameters();

            var nodes = context.WeChatRequestXmlData.SelectSingleNode("/xml")?.ChildNodes;
            if (nodes == null)
            {
                return;
            }

            foreach (XmlNode node in nodes)
            {
                if (node.GetType()!=typeof(XmlElement)||node.Name == "sign")
                {
                    continue;
                }

                parameters.AddParameter(node.Name, node.InnerText);
            }

            var responseSign = SignatureGenerator.Generate(parameters, MD5.Create(), options.ApiKey);

            if (responseSign != context.WeChatRequestXmlData.SelectSingleNode("/xml/sign")?.InnerText)
            {
                context.IsSuccess = false;
                context.FailedResponse = "订单签名验证没有通过";
                Logger.LogWarning("订单签名验证没有通过。");
            }
        }
    }
}