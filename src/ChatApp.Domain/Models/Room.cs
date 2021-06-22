using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatApp.Domain.Models
{
    public class Room
    {
        private readonly ISet<AnonymousUser> _anonymousUsers = new HashSet<AnonymousUser>();
        public IEnumerable<AnonymousUser> AnonymousUsers => _anonymousUsers;
        public Room(Tuple<AnonymousUser,AnonymousUser> anonymousUsers)
        {
            anonymousUsers.Item1.SetCurrentRoom(this);
            anonymousUsers.Item2.SetCurrentRoom(this);
            _anonymousUsers.Add(anonymousUsers.Item1);
            _anonymousUsers.Add(anonymousUsers.Item2);
        }
        public AnonymousUser GetOtherAnonymousUsers(AnonymousUser anonymousUser) => _anonymousUsers.SingleOrDefault(c => c.ConnectionId != anonymousUser.ConnectionId);

        public void Emptting()
        {
            foreach(var anonymousUser in _anonymousUsers)
            {
                anonymousUser.SetCurrentRoom(null);
                anonymousUser.ConnectionState = ConnectionState.Passive;
            }
        }
    }
}