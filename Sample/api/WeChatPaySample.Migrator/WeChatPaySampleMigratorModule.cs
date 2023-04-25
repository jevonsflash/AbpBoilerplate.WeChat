using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using WeChatPaySample.Configuration;
using WeChatPaySample.EntityFrameworkCore;
using WeChatPaySample.Migrator.DependencyInjection;

namespace WeChatPaySample.Migrator
{
    [DependsOn(typeof(WeChatPaySampleEntityFrameworkModule))]
    public class WeChatPaySampleMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public WeChatPaySampleMigratorModule(WeChatPaySampleEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(WeChatPaySampleMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                WeChatPaySampleConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus), 
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WeChatPaySampleMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
