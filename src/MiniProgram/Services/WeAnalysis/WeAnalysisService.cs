using Castle.MicroKernel.Registration;
using System.Net.Http;
using System.Threading.Tasks;

namespace WeChat.MiniProgram.Services.WeAnalysis
{
    /// <summary>
    /// 小程序统计服务。
    /// </summary>
    public class WeAnalysisService : CommonService
    {
        /// <summary>
        /// 获取访问页面数据。
        /// </summary>

        public Task<GetVisitPageResponse> GetVisitPageAsync(string begin_date, string end_date)
        {
            const string targetUrl = "https://api.weixin.qq.com/datacube/getweanalysisappidvisitpage";

            var request = new GetVisitPageRequest(begin_date, end_date);

            return WeChatMiniProgramApiRequester.RequestAsync<GetVisitPageResponse>(targetUrl,
                HttpMethod.Post, request);
        }
    }
}