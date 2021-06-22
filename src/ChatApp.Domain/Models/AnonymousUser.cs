namespace ChatApp.Domain.Models
{
    public class AnonymousUser
    {
        public string ConnectionId {get;}
        public ConnectionState ConnectionState {get; set;}
        public Room CurrentRoom {get; protected set;} = null;

        public AnonymousUser(string connectionId, ConnectionState connectionState = ConnectionState.Passive)
        {
            ConnectionId = connectionId;
            ConnectionState = connectionState;
        }

        public void SetCurrentRoom(Room room)
            => CurrentRoom = room;
    }
}