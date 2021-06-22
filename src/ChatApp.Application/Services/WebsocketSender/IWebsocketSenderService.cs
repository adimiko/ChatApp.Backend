using System.Threading.Tasks;
using ChatApp.Domain.Models;

namespace ChatApp.Application.Services.WebsocketSender
{
    public interface IWebsocketSenderService : IService
    {
        Task SendMessage(AnonymousUser anonymousUser, string message);
        Task SendMessageToRoom(Room room, string message);
        Task StartChatting(Room room);
        Task StopChatting(Room room);
    }
}