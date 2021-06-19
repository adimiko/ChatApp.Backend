using System.Threading.Tasks;
using ChatApp.Api.Models;

namespace ChatApp.Api.Hubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(string message);
        Task StartChatting(string message = "We found a person for you.");
        Task StopChatting();
    }
}