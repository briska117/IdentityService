using Microsoft.AspNetCore.Identity;

namespace IdentityService.Services.Auth
{
    public interface IAuthService
    {
        public Task<IdentityUser> Authenticate(string email, string password);
        public Task<string> GenerateToken(IdentityUser applicationUser);
    }
}
