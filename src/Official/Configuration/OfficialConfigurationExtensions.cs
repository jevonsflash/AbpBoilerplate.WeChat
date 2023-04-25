using Abp.Configuration.Startup;

namespace WeChat.Official.Configuration
{
    public static class OfficialConfigurationExtensions
    {
        /// <summary>
        ///     Used to configure ABP Official module.
        /// </summary>
        public static IOfficialConfiguration Official(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<IOfficialConfiguration>();
        }
    }
}
