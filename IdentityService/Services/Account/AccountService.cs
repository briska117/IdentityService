using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace IdentityService.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> userManager;

        public AccountService(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<IdentityUser> CreateUser(CreateAccountForm accountForm)
        {
            IdentityUser user = new IdentityUser { 
                Email = accountForm.Email,
                UserName = accountForm.Email

                };

            var identityResult = await this.userManager.CreateAsync(user, accountForm.Password);

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            identityResult = await userManager.ConfirmEmailAsync(user, token);
            ThrowIfFailedIdentityResult(identityResult);

            identityResult = await userManager.AddToRoleAsync(user, accountForm.Role.ToString());
            ThrowIfFailedIdentityResult(identityResult);

            return user;

        }

        /// <summary>
        /// Ensures the role.
        /// </summary>
        /// <param name="roleManager">The role manager.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        private static async Task EnsureRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            bool exists = await roleManager.RoleExistsAsync(roleName);
            if (!exists)
            {
                var role = new IdentityRole { Name = roleName };
                var identityResult = await roleManager.CreateAsync(role);
                ThrowIfFailedIdentityResult(identityResult);
            }
        }

        /// <summary>
        /// Throws if failed identity result.
        /// </summary>
        /// <param name="identityResult">The identity result.</param>
        /// <exception cref="InvalidOperationException"></exception>
        private static void ThrowIfFailedIdentityResult(IdentityResult identityResult)
        {
            if (!identityResult.Succeeded)
            {
                var sb = new StringBuilder();
                foreach (var error in identityResult.Errors)
                {
                    sb.AppendLine($"({error.Code}) {error.Description}");
                }

                throw new InvalidOperationException(sb.ToString());
            }
        }
    }
}
