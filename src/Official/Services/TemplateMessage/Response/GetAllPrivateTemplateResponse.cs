using System.Collections.Generic;
using WeChat.Official.Infrastructure.Models;
using Newtonsoft.Json;

namespace WeChat.Official.Services.TemplateMessage.Response
{
    public class GetAllPrivateTemplateResponse : OfficialCommonResponse
    {
        /// <summary>
        /// 模版列表数据。
        /// </summary>
        [JsonProperty("template_list")]
        public List<TemplateDefinition> TemplateList { get; set; }
    }
}