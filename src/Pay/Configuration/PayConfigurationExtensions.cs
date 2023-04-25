using Abp.Configuration.Startup;

namespace WeChat.Pay.Configuration
{
    public static class PayConfigurationExtensions
    {
        /// <summary>
        ///     Used to configure ABP Pay module.
        /// </summary>
        public static IPayConfiguration Pay(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<IPayConfiguration>();
        }
    }
}
