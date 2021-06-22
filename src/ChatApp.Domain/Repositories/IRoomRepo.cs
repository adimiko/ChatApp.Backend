using System.Threading.Tasks;
using ChatApp.Domain.Models;

namespace ChatApp.Domain.Repositories
{
    public interface IRoomRepo : IRepo
    {
        Task<Room> GetRoom(AnonymousUser memberOfRoom);
        Task AddRoom(Room room);
        Task DeleteAsync(Room room);
    }
}