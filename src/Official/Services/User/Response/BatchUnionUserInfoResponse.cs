using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeChat.Official.Services.User.Response
{
    public class BatchUnionUserInfoResponse : OfficialUserListResponse
    {
        [JsonProperty("user_info_list")]
        public List<UnionUserInfoResponse> UserInfoList { get; private set; }
    }
}