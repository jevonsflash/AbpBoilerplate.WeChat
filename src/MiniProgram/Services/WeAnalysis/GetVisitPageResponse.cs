using Newtonsoft.Json;
using WeChat.MiniProgram.Models;

namespace WeChat.MiniProgram.Services.WeAnalysis
{
    public class GetVisitPageResponse
    {
        /// <summary>
        /// ��ȡ���������ݵ����ڣ���ʽ��yyyyMMdd����
        /// </summary>
        [JsonProperty("ref_date")]
        public virtual string RefDateString { get; set; } = default!;

        /// <summary>
        /// ��ȡ�������û����ʷֲ������б�
        /// </summary>
        [JsonProperty("list")]
        public Data[] DataList { get; set; } = default!;

    }

    public class Data
    {
        /// <summary>
        /// ��ȡ������ҳ��·����
        /// </summary>
        [JsonProperty("page_path")]
        public string PagePath { get; set; } = default!;

        /// <summary>
        /// ��ȡ�����÷��ʴ�����
        /// </summary>
        [JsonProperty("page_visit_pv")]
        public int PageVisitPV { get; set; }

        /// <summary>
        /// ��ȡ�����÷���������
        /// </summary>
        [JsonProperty("page_visit_uv")]
        public int PageVisitUV { get; set; }

        /// <summary>
        /// ��ȡ�����ôξ�ͣ��ʱ������λ���룩��
        /// </summary>
        [JsonProperty("page_staytime_pv")]
        public double PageStayTimePerPV { get; set; }

        /// <summary>
        /// ��ȡ�����ý���ҳ������
        /// </summary>
        [JsonProperty("entrypage_pv")]
        public int EntryPagePV { get; set; }

        /// <summary>
        /// ��ȡ�������˳�ҳ������
        /// </summary>
        [JsonProperty("exitpage_pv")]
        public int ExitPagePV { get; set; }

        /// <summary>
        /// ��ȡ������ת��������
        /// </summary>
        [JsonProperty("page_share_pv")]
        public int PageSharePV { get; set; }

        /// <summary>
        /// ��ȡ������ת��������
        /// </summary>
        [JsonProperty("page_share_uv")]
        public int PageShareUV { get; set; }
    }
}