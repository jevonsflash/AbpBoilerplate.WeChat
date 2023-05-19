using Newtonsoft.Json;
using WeChat.MiniProgram.Models;

namespace WeChat.MiniProgram.Services.WeAnalysis
{
    public class GetVisitPageRequest : MiniProgramCommonRequest
    {
        /// <summary>
        /// 获取或设置开始时间字符串（格式：yyyyMMdd）。
        /// </summary>
        [JsonProperty("begin_date")]
        public string BeginDateString { get; set; } = string.Empty;

        /// <summary>
        /// 获取或设置结束时间字符串（格式：yyyyMMdd）。
        /// </summary>
        [JsonProperty("end_date")]
        public string EndDateString { get; set; } = string.Empty;

        public GetVisitPageRequest(string beginDateString, string endDateString)
        {
            BeginDateString = beginDateString;
            EndDateString = endDateString;
        }
    }
}