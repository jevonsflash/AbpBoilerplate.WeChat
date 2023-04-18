using System.Collections.Generic;
using WeChat.Official.Infrastructure.Models;
using Newtonsoft.Json;

namespace WeChat.Official.Services.CustomMenu.Request
{
    public class CreateCustomMenuRequest : OfficialCommonRequest
    {
        [JsonProperty("button")]
        public IList<CustomButton> Buttons { get; set; }
    }
}