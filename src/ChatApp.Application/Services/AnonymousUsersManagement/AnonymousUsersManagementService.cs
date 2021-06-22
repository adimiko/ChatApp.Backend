using System.Threading.Tasks;
using ChatApp.Domain.Models;
using ChatApp.Domain.Repositories;

namespace ChatApp.Application.Services.AnonymousUsersManagement
{
    public class AnonymousUsersManagementService : IAnonymousUsersManagementService
    {
        private readonly IAnonymousUserRepo _anonymousUserRepo;
        public AnonymousUsersManagementService(IAnonymousUserRepo anonymousUserRepo)
        {
            _anonymousUserRepo = anonymousUserRepo;
        }

        public async Task<AnonymousUser> GetAsync(string connectionId)
            => await _anonymousUserRepo.GetAsync(connectionId);

        public async Task CreateAsync(string connectionId)
            => await _anonymousUserRepo.AddAsync(new AnonymousUser(connectionId));


        public async Task RemoveAsync(AnonymousUser anonymousUser)
            => await _anonymousUserRepo.DeleteAsync(anonymousUser);
    } 
}