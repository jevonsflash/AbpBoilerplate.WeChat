using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatPaySample.WeChatPay
{
    public class WeChatSnsAuthenticateModel
    {

        public string State { get; set; }
        public string Redirect { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
