using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatApp.Domain.Models
{
    public class Lobby
    {
        private Queue<AnonymousUser> _queue = new Queue<AnonymousUser>();
        public IEnumerable<AnonymousUser> Queue => _queue;

        public Lobby()
        { }

        public void Add(AnonymousUser anonymousUser)
        {
            if(anonymousUser == null) throw new NullReferenceException();
            _queue.Enqueue(anonymousUser);
        }

        public Tuple<AnonymousUser, AnonymousUser> DequeuePairedUsers()
        {
            if(_queue.Count <= 1) return null;
            return new Tuple<AnonymousUser, AnonymousUser>(_queue.Dequeue(), _queue.Dequeue());
        }

        public void Remove(AnonymousUser anonymousUser)
        {
            if(anonymousUser == null) throw new NullReferenceException();
            _queue = new Queue<AnonymousUser>(_queue.Where(u => u.ConnectionId != anonymousUser.ConnectionId));
        }
    }
}