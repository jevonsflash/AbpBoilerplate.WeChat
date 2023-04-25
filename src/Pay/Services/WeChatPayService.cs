using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Abp.Dependency;
using WeChat.Pay.Exceptions;
using WeChat.Pay.Configuration;
using WeChat.Pay.Infrastructure;
using WeChat.Pay.Models;
using WeChat.Common.Infrastructure.Signature;
using Abp.Json;

namespace WeChat.Pay.Services
{
    /// <summary>
    /// 微信支付服务的基类定义，主要提供了常用组件的组入，例如签名生成组件等。
    /// </summary>
    public abstract class WeChatPayService : ITransientDependency
    {

        public WeChatPayService(IPayConfiguration options)
        {
            this.options = options;
        }
        public IServiceProvider ServiceProvider { get; set; }

        protected readonly object ServiceLocker = new object();
        private readonly IPayConfiguration options;

        protected TService LazyLoadService<TService>(ref TService service)
        {
            if (service == null)
            {
                lock (ServiceLocker)
                {
                    if (service == null)
                    {
                        service = ServiceProvider.GetRequiredService<TService>();
                    }
                }
            }

            return service;
        }

        protected ISignatureGenerator SignatureGenerator => LazyLoadService(ref _signatureGenerator);
        private ISignatureGenerator _signatureGenerator;

        protected IWeChatPayApiRequester WeChatPayApiRequester => LazyLoadService(ref _weChatPayApiRequester);
        private IWeChatPayApiRequester _weChatPayApiRequester;

        public ILoggerFactory LoggerFactory => LazyLoadService(ref _loggerFactory);
        private ILoggerFactory _loggerFactory;

        private Lazy<ILogger> _lazyLogger => new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);
        protected ILogger Logger => _lazyLogger.Value;


        protected IHttpClientFactory HttpClientFactory => LazyLoadService(ref _httpClientFactory);
        private IHttpClientFactory _httpClientFactory;

        protected virtual async Task<XmlDocument> RequestAndGetReturnValueAsync(string targetUrl, WeChatPayParameters requestParameters)
        {
            var result = await WeChatPayApiRequester.RequestAsync(targetUrl, requestParameters.ToXmlStr());
            if (result.SelectSingleNode("/xml/return_code")?.InnerText != "SUCCESS" ||
                result.SelectSingleNode("/xml/return_msg")?.InnerText != "OK")
            {
                var errMsg = $"微信支付调用失败-返回值:{result.SelectSingleNode("/xml/err_code_des")?.InnerText}  返回内容:{ result.SelectSingleNode("/xml/return_msg")?.InnerText}";
                Logger.Log(LogLevel.Error, errMsg, targetUrl, requestParameters);

                var exception = new CallWeChatPayApiException(errMsg);
                exception.Data.Add(nameof(targetUrl), targetUrl);
                exception.Data.Add(nameof(requestParameters), requestParameters);

                throw exception;
            }

            return result;
        }

        protected virtual async ValueTask<bool> CurrentIsSandboxModeAsync()
        {
            return options.IsSandBox;
        }

        protected virtual async ValueTask<string> GetRequestUrl(string standardUrl)
        {
            if (await CurrentIsSandboxModeAsync())
            {
                return Regex.Replace(standardUrl, "/pay/", "/sandboxnew/pay/");
            }

            return standardUrl;
        }
    }
}