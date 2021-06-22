using System.Threading.Tasks;
using ChatApp.Domain.Models;

namespace ChatApp.Application.Services.AnonymousUsersManagement
{
    public interface IAnonymousUsersManagementService : IService
    {
        Task<AnonymousUser> GetAsync(string connectionId);
        Task CreateAsync(string connectionId);
        Task RemoveAsync(AnonymousUser anonymousUser);
    }
}