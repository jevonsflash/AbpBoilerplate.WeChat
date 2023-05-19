using Newtonsoft.Json;
using WeChat.MiniProgram.Models;

namespace WeChat.MiniProgram.Services.WeAnalysis
{
    public class GetVisitPageResponse
    {
        /// <summary>
        /// 获取或设置数据的日期（格式：yyyyMMdd）。
        /// </summary>
        [JsonProperty("ref_date")]
        public virtual string RefDateString { get; set; } = default!;

        /// <summary>
        /// 获取或设置用户访问分布数据列表。
        /// </summary>
        [JsonProperty("list")]
        public Data[] DataList { get; set; } = default!;

    }

    public class Data
    {
        /// <summary>
        /// 获取或设置页面路径。
        /// </summary>
        [JsonProperty("page_path")]
        public string PagePath { get; set; } = default!;

        /// <summary>
        /// 获取或设置访问次数。
        /// </summary>
        [JsonProperty("page_visit_pv")]
        public int PageVisitPV { get; set; }

        /// <summary>
        /// 获取或设置访问人数。
        /// </summary>
        [JsonProperty("page_visit_uv")]
        public int PageVisitUV { get; set; }

        /// <summary>
        /// 获取或设置次均停留时长（单位：秒）。
        /// </summary>
        [JsonProperty("page_staytime_pv")]
        public double PageStayTimePerPV { get; set; }

        /// <summary>
        /// 获取或设置进入页次数。
        /// </summary>
        [JsonProperty("entrypage_pv")]
        public int EntryPagePV { get; set; }

        /// <summary>
        /// 获取或设置退出页次数。
        /// </summary>
        [JsonProperty("exitpage_pv")]
        public int ExitPagePV { get; set; }

        /// <summary>
        /// 获取或设置转发次数。
        /// </summary>
        [JsonProperty("page_share_pv")]
        public int PageSharePV { get; set; }

        /// <summary>
        /// 获取或设置转发人数。
        /// </summary>
        [JsonProperty("page_share_uv")]
        public int PageShareUV { get; set; }
    }
}