using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Domain.Models;
using ChatApp.Domain.Repositories;

namespace ChatApp.Infrastructure.Repos
{
    public class RoomRepo : IRoomRepo
    {
        private static IList<Room> _rooms = new List<Room>();

        public async Task<Room> GetRoom(AnonymousUser memberOfRoom)
            => await Task.FromResult(_rooms.SingleOrDefault(r => r.AnonymousUsers.Contains(memberOfRoom)));

        public async Task AddRoom(Room room)
        {
            _rooms.Add(room);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Room room)
        {
            _rooms.Remove(room);
            await Task.CompletedTask;
        }
    }
}