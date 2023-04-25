using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using WeChatPaySample.Authorization;

namespace WeChatPaySample
{
    [DependsOn(
        typeof(WeChatPaySampleCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class WeChatPaySampleApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<WeChatPaySampleAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(WeChatPaySampleApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
