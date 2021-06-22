using System.Threading.Tasks;
using ChatApp.Domain.Models;

namespace ChatApp.Application.Services.Chat
{
    public interface IChatService : IService
    {
        Task JoinToLobbyAsync(AnonymousUser anonymousUser);
        Task SendMessageToRoomAsync(AnonymousUser anonymousUser, string message);

        Task StopChattingAsync(AnonymousUser anonymousUser);
    }
}