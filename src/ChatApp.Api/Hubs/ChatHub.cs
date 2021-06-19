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
        private static ISet<Connection> _connections = new HashSet<Connection>();
        private static ISet<Room> _rooms = new HashSet<Room>();
        private Connection CurrentConnection =>_connections.SingleOrDefault(c => c.ConnectionId == Context.ConnectionId);
        private Room CallerRoom => _connections.SingleOrDefault(c => c == CurrentConnection).CurrentRoom;
        private static Queue<Connection> Queue = new Queue<Connection>();

        
        public override async Task OnConnectedAsync()
        {
            _connections.Add(new Connection(Context.ConnectionId, ConnectionState.Passive));
            await Clients.Caller.ReceiveMessage("Welcome on ChatApp. Join to lobby if you want to start chatting.");
            await base.OnConnectedAsync();
        }

        public async Task JoinToLobby()
        {
            if(CurrentConnection.ConnectionState == ConnectionState.Chatting)
            {
                throw new Exception("You are chatting !!!");
            }

            await Clients.Caller.ReceiveMessage("We are looking for a person for you...");
            Queue.Enqueue(CurrentConnection);

            bool IsReady = false;
            while(!IsReady)
            {
                await Task.Delay(1000);
                if(Queue.Count > 1) 
                {
                    
                    IsReady = true;
                    var firstConnection = Queue.Dequeue();
                    var secoundConnection = Queue.Dequeue();

                    firstConnection.ConnectionState = ConnectionState.Chatting;
                    secoundConnection.ConnectionState = ConnectionState.Chatting;
                    _rooms.Add(new Room(firstConnection, secoundConnection));

                    await Clients.Client(firstConnection.ConnectionId).StartChatting();
                    await Clients.Client(secoundConnection.ConnectionId).StartChatting();
                }
            }
        }

        public async Task StopChatting()
        {
            var otherPerson = CallerRoom.GetOtherPerson(CurrentConnection);
            await Clients.Client(otherPerson.ConnectionId).StopChatting();
            DestroyRoom();
        }

        public async Task SendMessage(string message)
        {
            if(string.IsNullOrWhiteSpace(message)) return;
            var otherPerson = CallerRoom.GetOtherPerson(CurrentConnection);
            await Clients.Client(CurrentConnection.ConnectionId).ReceiveMessage(message);
            await Clients.Client(otherPerson.ConnectionId).ReceiveMessage(message);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await StopChatting();

            if(CurrentConnection != null)_connections.Remove(CurrentConnection);
            await base.OnDisconnectedAsync(exception);
        }

        private void DestroyRoom()
        {
            var otherPerson = CallerRoom.GetOtherPerson(CurrentConnection);
            CurrentConnection.SetCurrentRoom(null);
            otherPerson.SetCurrentRoom(null);
            CurrentConnection.ConnectionState = ConnectionState.Passive;
            otherPerson.ConnectionState = ConnectionState.Passive;
            _rooms.Remove(CallerRoom);
        }


    }
}