using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatPaySample.Pay
{
    public class PrePayInput  : EntityDto<long>
    {
        [Required]
        public long UserId { get; set; }
        public string Type { get; set; }
        public string LoginProvider { get; set; }



    }
}
