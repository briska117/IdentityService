using IdentityService.Data;
using IdentityService.Models;

namespace IdentityService.Repositories.Account
{
    public class AccountRepositorie : IAccountRepositorie
    {
        private readonly ApplicationDbContext applicationDbContext;

        public AccountRepositorie(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public Task<List<AccountDto>> GetAccount()
        {
            throw new NotImplementedException();
            //return this.
        }
        public Task<AccountDto> UpdateAccount(AccountDto accountDto)
        {
            throw new NotImplementedException();
        }
    }
}
