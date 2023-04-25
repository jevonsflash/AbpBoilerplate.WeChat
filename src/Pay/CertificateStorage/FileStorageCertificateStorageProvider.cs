using Abp.Dependency;
using Abp.Threading;
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
    public class FileStorageCertificateStorageProvider : ICertificateStorageProvider
    {
        private readonly BlobManager blobContainer;
        private readonly IPayConfiguration payConfiguration;

        public FileStorageCertificateStorageProvider(BlobManager blobManager, IPayConfiguration payConfiguration)
        {
            this.blobContainer = blobManager;
            this.payConfiguration = payConfiguration;
        }
        public async void Initialize()
        {

            var blobName = payConfiguration.CertificateBlobName;
            var certificateBlobContainerName = payConfiguration.CertificateBlobContainerName;
            blobContainer.SetContainerName(certificateBlobContainerName);
            if (await blobContainer.ExistsAsync(blobName))
            {
                return;
            }
            var assets = string.IsNullOrEmpty(payConfiguration.CertificateAssetsName) ? $"WeChat.Pay.Assets.{blobName}.p12" : payConfiguration.CertificateAssetsName;
            await InitAppCertFile(blobContainer, blobName, assets);

        }

        public async Task<byte[]> GetBytesAsync()
        {
            byte[] allBytes;
            using (var stream = await blobContainer.GetAsync(payConfiguration.CertificateBlobName))
            {
                using (var memoryStream = new MemoryStream())
                {
                    if (stream.CanSeek)
                    {
                        stream.Position = 0;
                    }
                    await stream.CopyToAsync(memoryStream);
                    allBytes = memoryStream.ToArray();
                }
            }
            return allBytes;
        }


        public byte[] GetBytes()
        {
            var certificateBytes = AsyncHelper.RunSync(async () =>
            {
                byte[] allBytes;
                using (var stream = await blobContainer.GetAsync(payConfiguration.CertificateBlobName))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        if (stream.CanSeek)
                        {
                            stream.Position = 0;
                        }
                        await stream.CopyToAsync(memoryStream);
                        allBytes = memoryStream.ToArray();
                    }
                }
                return allBytes;

            });

            return certificateBytes;
        }

        private async Task InitAppCertFile(BlobManager blobContainer, string blobName, string fileName)
        {
            using (var memoryStream = Assembly.GetExecutingAssembly()
                                .GetManifestResourceStream(fileName))
            {
                await blobContainer.SaveAsync(blobName, memoryStream, true);
            }
        }
    }
}
