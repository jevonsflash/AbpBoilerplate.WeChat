using Abp.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using WeChat.Common;
using System.Reflection;
using Abp.Reflection.Extensions;
using WeChat.Common.Configuration;
using Abp.Threading;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using WeChat.Pay.Configuration;
using Abp.Dependency;
using FileStorage.Blob;
using FileStorage.Configuration;
using System.Threading.Tasks;

namespace WeChat.Pay
{
    [DependsOn(typeof(WeChatCommonModule))]
    public class PayModule : AbpModule
    {

        private readonly IConfigurationRoot _appConfiguration;
        public PayModule(IHostEnvironment env)
        {
            _appConfiguration = Common.Configuration.AppConfigurations.Get(
    typeof(PayModule).GetAssembly().GetDirectoryPathOrNull(), env.EnvironmentName, env.IsDevelopment());
        }

        public override void PreInitialize()
        {
            IocManager.Register<IPayConfiguration, PayConfiguration>();
            Configuration.Modules.Pay().ApiKey = _appConfiguration["WeChat:Pay:ApiKey"];
            Configuration.Modules.Pay().MchId = _appConfiguration["WeChat:Pay:MchId"];
            Configuration.Modules.Pay().IsSandBox = bool.Parse(_appConfiguration["WeChat:Pay:IsSandBox"]);
            Configuration.Modules.Pay().NotifyUrl = _appConfiguration["WeChat:Pay:NotifyUrl"];
            Configuration.Modules.Pay().RefundNotifyUrl = _appConfiguration["WeChat:Pay:RefundNotifyUrl"];
            Configuration.Modules.Pay().CertificateBlobContainerName = _appConfiguration["WeChat:Pay:CertificateBlobContainerName"];
            Configuration.Modules.Pay().CertificateBlobName = _appConfiguration["WeChat:Pay:CertificateBlobName"];
            Configuration.Modules.Pay().CertificateSecret = _appConfiguration["WeChat:Pay:CertificateSecret"];
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override async void PostInitialize()
        {
            using (var blobContainerWrapper = IocManager.ResolveAsDisposable<BlobManager>())
            {
                var blobContainer = blobContainerWrapper.Object;

                var blobName = Configuration.Modules.Pay().CertificateBlobName;
                var certificateBlobContainerName = Configuration.Modules.Pay().CertificateBlobContainerName;
                blobContainer.SetContainerName(certificateBlobContainerName);
                if (await blobContainer.ExistsAsync(blobName))
                {
                    return;
                }

                await InitAppCertFile(blobContainer, blobName, $"WeChat.Pay.Assets.{blobName}.p12");
            }

            base.PostInitialize();
        }

        private static async Task InitAppCertFile(BlobManager blobContainer, string blobName, string fileName)
        {
            using (var memoryStream = Assembly.GetExecutingAssembly()
                                .GetManifestResourceStream(fileName))
            {
                await blobContainer.SaveAsync(blobName, memoryStream, true);
            }
        }
    }
}