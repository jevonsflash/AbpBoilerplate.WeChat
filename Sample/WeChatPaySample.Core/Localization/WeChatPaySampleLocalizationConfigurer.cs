using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace WeChatPaySample.Localization
{
    public static class WeChatPaySampleLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(WeChatPaySampleConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(WeChatPaySampleLocalizationConfigurer).GetAssembly(),
                        "WeChatPaySample.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
