using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml;
using Abp.Dependency;
using Abp.Threading;
using WeChat.Pay.Configuration;
using Abp.Extensions;
using FileStorage.Blob;
using System.Threading;
using System.Data.SqlTypes;

namespace WeChat.Pay.Infrastructure
{
    public class DefaultWeChatPayApiRequester : IWeChatPayApiRequester, ITransientDependency
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IPayConfiguration options;
        private readonly BlobManager blobManager;

        public DefaultWeChatPayApiRequester(
            IHttpClientFactory httpClientFactory,
            IPayConfiguration payConfiguration,
            BlobManager blobManager


            )
        {
            _httpClientFactory = httpClientFactory;
            this.options=payConfiguration;
            blobManager.SetContainerName(this.options.CertificateBlobContainerName);
            this.blobManager=blobManager;
        }

        public async Task<XmlDocument> RequestAsync(string url, string body)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(body)
            };

            var client = new HttpClient(GetWeChatPayHandler());



            var responseMessage = await client.SendAsync(request);
            var readAsString = await responseMessage.Content.ReadAsStringAsync();

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"微信支付接口请求失败。\n错误码: {responseMessage.StatusCode}，\n响应内容: {readAsString}");
            }

            var newXmlDocument = new XmlDocument();
            try
            {
                newXmlDocument.LoadXml(readAsString);
            }
            catch (XmlException e)
            {
                throw new HttpRequestException($"请求接口失败，返回的不是一个标准的 XML 文档。\n响应内容: {readAsString}");
            }

            return newXmlDocument;
        }


        public HttpClientHandler GetWeChatPayHandler()
        {
            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls
            };

            if (string.IsNullOrEmpty(options.CertificateBlobName))
                return handler;




            var certificateBytes = AsyncHelper.RunSync(async () =>
            {
                byte[] allBytes;
                using (var stream = await blobManager.GetAsync(options.CertificateBlobName))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        if (stream.CanSeek)
                        {
                            stream.Position = 0;
                        }
                        await stream.CopyToAsync(memoryStream);
                        allBytes = memoryStream.ToArray();
                    }
                }
                return allBytes;

            });
            if (certificateBytes == null) throw new FileNotFoundException("证书文件不存在");

            handler.ClientCertificates.Add(new X509Certificate2(
                certificateBytes,
                options.CertificateSecret,
                X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet));
            handler.ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true;

            return handler;
        }
    }
}