using System.Threading.Tasks;
using ChatApp.Domain.Models;
using ChatApp.Domain.Repositories;

namespace ChatApp.Infrastructure.Repos
{
    public class LobbyRepo : ILobbyRepo
    {
        private static Lobby _lobby = new Lobby();
        public async Task<Lobby> GetLobby()
            => await Task.FromResult(_lobby);

        public async Task UpdateLobby(Lobby lobby)
        {
            _lobby = lobby;
            await Task.CompletedTask;
        }
    }
}