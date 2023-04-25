using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using WeChatPaySample.Configuration;

namespace WeChatPaySample.Web.Host.Startup
{
    [DependsOn(
       typeof(WeChatPaySampleWebCoreModule))]
    public class WeChatPaySampleWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public WeChatPaySampleWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WeChatPaySampleWebHostModule).GetAssembly());
        }
    }
}
