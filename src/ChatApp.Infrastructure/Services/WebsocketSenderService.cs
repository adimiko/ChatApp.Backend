using System.Threading.Tasks;
using ChatApp.Domain.Models;
using ChatApp.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using ChatApp.Application.Services.WebsocketSender;

namespace ChatApp.Infrastructure.Services
{
    public class WebsocketSenderService : IWebsocketSenderService
    {
        private readonly IHubContext<ChatHub, IChatClient> _context;
        public WebsocketSenderService(IHubContext<ChatHub, IChatClient> context)
            => _context = context;

        public async Task SendMessage(AnonymousUser anonymousUser, string message)
            => await _context.Clients.Client(anonymousUser.ConnectionId).ReceiveMessage(message);

        public async Task StartChatting(Room room)
        {
            foreach(var anonymousUser in room.AnonymousUsers) await _context.Clients.Client(anonymousUser.ConnectionId).StartChatting();
        }

        public async Task SendMessageToRoom(Room room, string message)
        {
            foreach(var anonymousUser in room.AnonymousUsers) await _context.Clients.Client(anonymousUser.ConnectionId).ReceiveMessage(message);
        }

        public async Task StopChatting(Room room)
        {
            foreach(var anonymousUser in room.AnonymousUsers) await _context.Clients.Client(anonymousUser.ConnectionId).StopChatting();
        }


    }
}