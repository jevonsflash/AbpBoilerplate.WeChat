using Abp.Localization;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Security;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using WeChat.MiniProgram;
using WeChat.Official;
using WeChat.Pay;
using WeChat.Pay.CertificateStorage;
using WeChatPaySample.Authorization.Roles;
using WeChatPaySample.Authorization.Users;
using WeChatPaySample.CertificateStorage;
using WeChatPaySample.Configuration;
using WeChatPaySample.Localization;
using WeChatPaySample.MultiTenancy;
using WeChatPaySample.Timing;

namespace WeChatPaySample
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    [DependsOn(typeof(PayModule))]
    [DependsOn(typeof(MiniProgramModule))]
    [DependsOn(typeof(OfficialModule))]
    public class WeChatPaySampleCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            WeChatPaySampleLocalizationConfigurer.Configure(Configuration.Localization);

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = WeChatPaySampleConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();
            
            Configuration.Localization.Languages.Add(new LanguageInfo("fa", "فارسی", "famfamfam-flags ir"));
            
            Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase = WeChatPaySampleConsts.DefaultPassPhrase;
            SimpleStringCipher.DefaultPassPhrase = WeChatPaySampleConsts.DefaultPassPhrase;


            //Sample：更改CertificateStorageProvider实现
            //IocManager.Register<ICertificateStorageProvider, MyCertificateStorageProvider>();

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WeChatPaySampleCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
