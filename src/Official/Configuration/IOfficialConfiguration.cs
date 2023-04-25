namespace WeChat.Official.Configuration
{
    public interface IOfficialConfiguration
    {
        /// <summary>
        /// 微信公众号的 AppId。
        /// </summary>
        string AppId { get; set; }

        /// <summary>
        /// 微信公众号的 API Secret。
        /// </summary>
        string AppSecret { get; set; }
    }
}