using System.Collections.Generic;
using System.Threading.Tasks;
using ChatApp.Domain.Models;

namespace ChatApp.Domain.Repositories
{
    public interface IAnonymousUserRepo : IRepo
    {
        Task<AnonymousUser> GetAsync(string connectionId);
        Task<IEnumerable<AnonymousUser>> BrowseAsync();
        Task AddAsync(AnonymousUser anonymousUser);
        Task UpdateAsync(AnonymousUser anonymousUser);
        Task DeleteAsync(AnonymousUser anonymousUser);
    }
}