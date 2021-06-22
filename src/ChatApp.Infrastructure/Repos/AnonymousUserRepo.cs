using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Domain.Models;
using ChatApp.Domain.Repositories;

namespace ChatApp.Infrastructure.Repos
{
    public class AnonymousUserRepo : IAnonymousUserRepo
    {
        private static ISet<AnonymousUser> _anonymousUsers = new HashSet<AnonymousUser>();

        public async Task<AnonymousUser> GetAsync(string connectionId)
            => await Task.FromResult(_anonymousUsers.SingleOrDefault(u => u.ConnectionId == connectionId));

        public async Task<IEnumerable<AnonymousUser>> BrowseAsync()
            => await Task.FromResult(_anonymousUsers);

        public async Task AddAsync(AnonymousUser anonymousUser)
        {
            _anonymousUsers.Add(anonymousUser);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(AnonymousUser anonymousUser)
        {
            _anonymousUsers.Remove(anonymousUser);
            await Task.CompletedTask;
        }



        public Task UpdateAsync(AnonymousUser anonymousUser)
        {
            throw new System.NotImplementedException();
        }
    }
}