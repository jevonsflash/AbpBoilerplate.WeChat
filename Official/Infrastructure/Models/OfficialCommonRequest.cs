using Newtonsoft.Json;

namespace WeChat.Official.Infrastructure.Models
{
    public abstract class OfficialCommonRequest : IOfficialRequest
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
        }
    }
}