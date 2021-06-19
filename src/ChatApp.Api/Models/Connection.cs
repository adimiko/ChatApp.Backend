namespace ChatApp.Api.Models
{
    public class Connection
    {
        public string ConnectionId {get;}
        public ConnectionState ConnectionState {get; set;}
        public Room CurrentRoom {get; protected set;} = null;

        public Connection(string connectionId, ConnectionState connectionState)
        {
            ConnectionId = connectionId;
            ConnectionState = connectionState;
        }

        public void SetCurrentRoom(Room room)
        {
            CurrentRoom = room;
        }
    }
}