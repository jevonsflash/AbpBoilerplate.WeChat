namespace WeChat.MiniProgram.Configuration
{
    public class MiniProgramConfiguration : IMiniProgramConfiguration
    {

        /// <summary>
        /// 微信公众号的 AppId。
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 微信公众号的 API Secret。
        /// </summary>
        public string AppSecret { get; set; }

    }
}