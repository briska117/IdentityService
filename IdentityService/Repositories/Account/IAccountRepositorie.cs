using IdentityService.Models;

namespace IdentityService.Repositories.Account
{
    public interface IAccountRepositorie
    {
        public Task<List<AccountDto>> GetAccount();
        public Task<AccountDto> UpdateAccount(AccountDto accountDto);
    }
}
