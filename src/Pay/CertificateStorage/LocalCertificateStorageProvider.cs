using Abp.Dependency;
using FileStorage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeChat.Pay.Configuration;

namespace WeChat.Pay.CertificateStorage
{
    public class LocalCertificateStorageProvider : ICertificateStorageProvider
    {
        private readonly IPayConfiguration payConfiguration;

        public LocalCertificateStorageProvider(IPayConfiguration payConfiguration)
        {
            this.payConfiguration = payConfiguration;
        }

        public byte[] GetBytes()
        {
            if (!File.Exists(payConfiguration.CertificateFilePath))
                throw new NotImplementedException();

            return File.ReadAllBytes(payConfiguration.CertificateFilePath);
        }

        public Task<byte[]> GetBytesAsync()
        {

            if (!File.Exists(payConfiguration.CertificateFilePath))
                throw new NotImplementedException();

            return File.ReadAllBytesAsync(payConfiguration.CertificateFilePath);
        }

        public void Initialize()
        {
        }
    }
}
