using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Api.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Api.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        private readonly IChatService _chatService;

        private Connection CurrentConnection => _chatService.Connections.SingleOrDefault(c => c.ConnectionId == Context.ConnectionId);
        private Room CallerRoom => _chatService.Connections.SingleOrDefault(c => c == CurrentConnection).CurrentRoom;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }
        public override async Task OnConnectedAsync()
        {
            _chatService.AddConnection(new Connection(Context.ConnectionId, ConnectionState.Passive), async (caller, message) =>
            {
                await Clients.Client(caller.ConnectionId).ReceiveMessage(message);
            });
            await base.OnConnectedAsync();
        }

        public void JoinToLobby()
        {
            _chatService.AddToLobby(_chatService.Connections.SingleOrDefault(c => c.ConnectionId == Context.ConnectionId), async (caller,message) =>
            {
                await Clients.Client(caller.ConnectionId).ReceiveMessage(message);
            });

            _chatService.TryCreateRoom(async (f,s)=>{
                await Clients.Client(f.ConnectionId).StartChatting();
                await Clients.Client(s.ConnectionId).StartChatting();
            });            
        }

        public void SendMessage(string message)
        {
            _chatService.SendMessage(CurrentConnection, message, async (f ,s) =>{
                await Clients.Client(f.ConnectionId).ReceiveMessage(message);
                await Clients.Client(s.ConnectionId).ReceiveMessage(message);
            });
        }

        public void StopChatting()
        {
            _chatService.StopChatting(CurrentConnection,async c =>{
                await Clients.Client(c.ConnectionId).StopChatting();
            });
        }



        public override async Task OnDisconnectedAsync(Exception exception)
        {
            StopChatting();

            if(CurrentConnection != null)_chatService.RemoveConnection(CurrentConnection);
            await base.OnDisconnectedAsync(exception);
        }
    }
}