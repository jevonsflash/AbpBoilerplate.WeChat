﻿namespace WeChat.Official.Configuration
{
    public class OfficialConfiguration : IOfficialConfiguration
    {
        public string Token { get; set; }

        public string AppId { get; set; }

        public string AppSecret { get; set; }

        public string EncodingAesKey { get; set; }

        public string OAuthRedirectUrl { get; set; }
    }
}