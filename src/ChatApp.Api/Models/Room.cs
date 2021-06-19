using System;
using System.Collections.Generic;
using System.Linq;

// Te samo połączenie w pokoju (w pokoju muszą być różne połączenia)
// Pokój jest pełny
// Pokój jest pusty
namespace ChatApp.Api.Models
{
    public class Room
    {
        private ISet<Connection> _connections = new HashSet<Connection>();
        public Room(Connection firstConnection, Connection secondConnection)
        {
            firstConnection.SetCurrentRoom(this);
            secondConnection.SetCurrentRoom(this);
            _connections.Add(firstConnection);
            _connections.Add(secondConnection);
        }

        public Connection GetOtherPerson(Connection connection) => _connections.SingleOrDefault(c => c.ConnectionId != connection.ConnectionId);
    }
}