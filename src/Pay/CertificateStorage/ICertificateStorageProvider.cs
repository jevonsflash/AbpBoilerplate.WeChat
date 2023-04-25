using Abp.Dependency;
using System.Threading.Tasks;

namespace WeChat.Pay.CertificateStorage
{
    public interface ICertificateStorageProvider : ITransientDependency
    {
        byte[] GetBytes();
        Task<byte[]> GetBytesAsync();
        void Initialize();
    }
}