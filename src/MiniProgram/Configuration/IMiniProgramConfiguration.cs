namespace WeChat.MiniProgram.Configuration
{
    public interface IMiniProgramConfiguration
    {

        /// <summary>
        /// 小程序 AppId。
        /// </summary>
        string AppId { get; set; }

        /// <summary>
        /// 小程序 API Secret。
        /// </summary>
        string AppSecret { get; set; }

    }
}