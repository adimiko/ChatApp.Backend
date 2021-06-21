using System;
using System.Collections.Generic;
using ChatApp.Api.Models;

namespace ChatApp.Api
{
    public interface IChatService
    {
        IEnumerable<Connection> Connections {get;}
        IEnumerable<Connection> Lobby {get;}
        IEnumerable<Room> Rooms {get;}

        void AddConnection(Connection connection, Action<Connection,string> sendMessageToCaller);
        void RemoveConnection(Connection connection);
        void AddToLobby(Connection connection, Action<Connection,string> sendMessageToCaller);
        void TryCreateRoom(Action<Connection,Connection> startChatting);
        void StopChatting(Connection connection, Action<Connection> stopChatting);
        void SendMessage(Connection connection, string message, Action<Connection,Connection> sendMessageToRoom);
    }
}