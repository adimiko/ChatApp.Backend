using System;
using System.Threading.Tasks;
using ChatApp.Application.Services.AnonymousUsersManagement;
using ChatApp.Application.Services.Chat;

using ChatApp.Domain.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Infrastructure.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        private readonly IChatService _chatService;
        private readonly IAnonymousUsersManagementService _anonymousUsersManagement;
        private async  Task<AnonymousUser> CurrentAnonymousUser() => await _anonymousUsersManagement.GetAsync(Context.ConnectionId);
        
        public ChatHub(IChatService chatService, IAnonymousUsersManagementService anonymousUsersManagement)
        {
            _chatService = chatService;
            _anonymousUsersManagement = anonymousUsersManagement;
        }
        public override async Task OnConnectedAsync()
        {
            await _anonymousUsersManagement.CreateAsync(Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public async Task JoinToLobby()
            => await _chatService.JoinToLobbyAsync(await CurrentAnonymousUser());

        public async Task SendMessage(string message)
            => await _chatService.SendMessageToRoomAsync(await CurrentAnonymousUser(), message);

        public async Task StopChatting()
            => await _chatService.StopChattingAsync(await CurrentAnonymousUser());

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await _chatService.RemoveFromLobbyAsync(await CurrentAnonymousUser());
            await _anonymousUsersManagement.RemoveAsync(await CurrentAnonymousUser());
            //remove room
            await base.OnDisconnectedAsync(exception);
        }
    }
}