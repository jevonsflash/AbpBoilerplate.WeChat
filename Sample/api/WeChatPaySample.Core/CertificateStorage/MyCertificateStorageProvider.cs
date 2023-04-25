using Abp.Dependency;
using FileStorage.Blob;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeChat.Pay.CertificateStorage;
using WeChat.Pay.Configuration;

namespace WeChatPaySample.CertificateStorage
{
    public class MyCertificateStorageProvider : ICertificateStorageProvider
    {


        public byte[] GetBytes()
        {
            var url = "http://api.xxxx.com/get-certificate";
            byte[] fileBytes = default;
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Authorization", "xxxx");
                var formData = new NameValueCollection();
                formData["file_id"] = "xxxxx";
                fileBytes = webClient.UploadValues(url, "POST", formData);
            }
            return fileBytes;
        }

        public async Task<byte[]> GetBytesAsync()
        {

            var url = "http://api.xxxx.com/get-certificate";
            byte[] fileBytes = default;
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Authorization", "xxxx");
                var formData = new NameValueCollection();
                formData["file_id"] = "xxxxx";
                fileBytes = await webClient.UploadValuesTaskAsync(url, "POST", formData);
            }
            return fileBytes;
        }

        public void Initialize()
        {
        }
    }
}
