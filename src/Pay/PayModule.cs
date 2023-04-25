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
using System.Threading.Tasks;
using FileStorage;
using WeChat.Pay.CertificateStorage;
using System;

namespace WeChat.Pay
{
    [DependsOn(typeof(WeChatCommonModule))]
    [DependsOn(typeof(FileStorageModule))]
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
            Configuration.Modules.Pay().CertificateAssetsName = _appConfiguration["WeChat:Pay:CertificateAssetsName"];
            Configuration.Modules.Pay().CertificateFilePath = _appConfiguration["WeChat:Pay:CertificateFilePath"];

            IocManager.RegisterIfNot<ICertificateStorageProvider, FileStorageCertificateStorageProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override async void PostInitialize()
        {
            using (var certificateStorageProvider = IocManager.ResolveAsDisposable<ICertificateStorageProvider>())
            {
                if (certificateStorageProvider.Object == null)
                {
                    throw new NotImplementedException("未注册ICertificateStorageProvider");
                }
                certificateStorageProvider.Object.Initialize();
            }

            base.PostInitialize();
        }

    }
}