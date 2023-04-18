using System.Threading.Tasks;

namespace WeChat.Official.Infrastructure
{
    public interface IAccessTokenAccessor
    {
        Task<string> GetAccessTokenAsync();
    }
}