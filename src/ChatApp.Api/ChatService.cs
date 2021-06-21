using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Api.Hubs;
using ChatApp.Api.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Api
{
    public class ChatService : IChatService
    {
        private ISet<Connection> _connections = new HashSet<Connection>();
        public Queue<Connection> _lobby = new Queue<Connection>();
        private ISet<Room> _rooms = new HashSet<Room>();

        public IEnumerable<Connection> Connections => _connections;
        public IEnumerable<Connection> Lobby => _lobby;
        public IEnumerable<Room> Rooms => _rooms;


        public ChatService(IHubContext<ChatHub,IChatClient> _context)
        {
            _context.Clients.Client("");
        }

        public void AddConnection(Connection connection, Action<Connection,string> sendMessageToCaller)
        {
            if(connection == null) throw new NullReferenceException("Connection cannot be null.");
            _connections.Add(connection);
            sendMessageToCaller(connection, "Welcome on ChatApp. Join to lobby if you want to start chatting.");
        }

        public void RemoveConnection(Connection connection)
        {
            if(connection == null) throw new NullReferenceException("Connection cannot be null.");
            _connections.Remove(connection);
        }

        public void AddToLobby(Connection connection, Action<Connection,string> sendMessageToCaller)
        {
            if(connection == null) throw new NullReferenceException("Connection cannot be null.");
            if(connection.ConnectionState == ConnectionState.Chatting)throw new Exception("You are chatting !!!");
            if(connection.ConnectionState == ConnectionState.LookingForRoom) return;
            connection.ConnectionState = ConnectionState.LookingForRoom;
            sendMessageToCaller(connection, "We are looking for a person for you...");
            _lobby.Enqueue(connection);
        }

        public void TryCreateRoom(Action<Connection,Connection> startChatting)
        {
            if(_lobby.Count > 1)
            {
                var firstConnection = _lobby.Dequeue();
                var secondConnection = _lobby.Dequeue();

                firstConnection.ConnectionState = ConnectionState.Chatting;
                secondConnection.ConnectionState = ConnectionState.Chatting;

                _rooms.Add(new Room(firstConnection, secondConnection));
                startChatting(firstConnection,secondConnection);
            }
        }


        public void SendMessage(Connection connection, string message, Action<Connection,Connection> sendMessageToRoom)
        {
            if(connection.ConnectionState != ConnectionState.Chatting) throw new Exception("You cannot chatting. Join to lobby.");
            if(string.IsNullOrWhiteSpace(message)) return;

            Room CallerRoom = _connections.SingleOrDefault(c => c == connection).CurrentRoom;
            var secondConnection = CallerRoom.GetOtherPerson(connection);

            sendMessageToRoom(connection, secondConnection);
        }

        public void StopChatting(Connection connection, Action<Connection> stopChatting)
        {
            var secondConnection = _connections.SingleOrDefault(c => c == connection).CurrentRoom.GetOtherPerson(connection);
            stopChatting(secondConnection);

            connection.ConnectionState = ConnectionState.Passive;
            secondConnection.ConnectionState = ConnectionState.Passive;

            connection.SetCurrentRoom(null);
            secondConnection.SetCurrentRoom(null);

            _rooms.Remove(GetRoom(connection));
        }

        private Room GetRoom(Connection connection) => _connections.SingleOrDefault(c => c == connection).CurrentRoom;

    }
}