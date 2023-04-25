using Abp.Modules;
using Microsoft.Extensions.Configuration;
using WeChat.Common;
using WeChat.Common.Configuration;
using WeChat.Official.Configuration;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace WeChat.Official
{
    [DependsOn(typeof(WeChatCommonModule))]
    public class OfficialModule : AbpModule 
    {
        private readonly IConfigurationRoot _appConfiguration;
        public OfficialModule(IHostEnvironment env)
        {
            _appConfiguration = AppConfigurations.Get(
    typeof(OfficialModule).GetAssembly().GetDirectoryPathOrNull(), env.EnvironmentName, env.IsDevelopment()
);
        }
        public override void PreInitialize()
        {
            IocManager.Register<IOfficialConfiguration, OfficialConfiguration>();
            Configuration.Modules.Official().AppId = _appConfiguration["WeChat:Official:AppId"];
            Configuration.Modules.Official().AppSecret = _appConfiguration["WeChat:Official:AppSecret"];
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}