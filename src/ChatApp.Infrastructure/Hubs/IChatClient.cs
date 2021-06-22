using System.Threading.Tasks;

namespace ChatApp.Infrastructure.Hubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(string message);
        Task StartChatting(string message = "We found a person for you.");
        Task StopChatting();
    }
}