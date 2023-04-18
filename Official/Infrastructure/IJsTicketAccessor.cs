using System.Threading.Tasks;

namespace WeChat.Official.Infrastructure
{
    public interface IJsTicketAccessor
    {
        Task<string> GetTicketJsonAsync();

        Task<string> GetTicketAsync();
    }
}