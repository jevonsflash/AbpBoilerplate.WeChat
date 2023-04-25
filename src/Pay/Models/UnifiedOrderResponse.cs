using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WeChat.Pay.Models
{
    public class UnifiedOrderResponse
    {

        public string appid { set; get; }
        public string partnerid { set; get; }
        public string prepayid { set; get; }
        public string package { set; get; }
        public string noncestr { set; get; }
        public string timestamp { set; get; }
        public string sign { set; get; }
    }
}
