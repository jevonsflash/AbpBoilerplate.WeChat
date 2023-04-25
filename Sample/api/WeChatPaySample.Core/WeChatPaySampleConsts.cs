using WeChatPaySample.Debugging;

namespace WeChatPaySample
{
    public class WeChatPaySampleConsts
    {
        public const string LocalizationSourceName = "WeChatPaySample";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "9e7fb01a9457417eb89b3ab152ea10a3";
    }
}
