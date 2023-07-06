using IdentityService.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Services.Account
{
    public interface IAccountService
    {
        public Task<IdentityUser> CreateUser(CreateAccountForm accountForm);
    }
}
