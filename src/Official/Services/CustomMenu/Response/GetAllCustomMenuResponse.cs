using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeChat.Official.Services.CustomMenu.Response
{
    public class GetAllCustomMenuResponse
    {
        [JsonProperty("menu")] 
        public GetAllCustomMenuInner Menu { get; set; }
    }
    
    public class GetAllCustomMenuInner
    {
        [JsonProperty("button")] 
        public List<CustomButton> Buttons { get; set; }
    }
}