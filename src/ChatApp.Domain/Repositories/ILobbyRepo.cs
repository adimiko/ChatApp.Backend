using System.Threading.Tasks;
using ChatApp.Domain.Models;

namespace ChatApp.Domain.Repositories
{
    public interface ILobbyRepo : IRepo
    {
        Task<Lobby> GetLobby();
        Task UpdateLobby(Lobby lobby);
    }
}