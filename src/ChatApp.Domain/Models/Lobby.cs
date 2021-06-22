using System;
using System.Collections.Generic;

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
            if(anonymousUser == null) throw new NullReferenceException("Anonymous user cannot be null.");
            _queue.Enqueue(anonymousUser);
        }

        public Tuple<AnonymousUser, AnonymousUser> DequeuePairedUsers()
        {
            if(_queue.Count <= 1) return null;
            return new Tuple<AnonymousUser, AnonymousUser>(_queue.Dequeue(), _queue.Dequeue());
        }
    }
}