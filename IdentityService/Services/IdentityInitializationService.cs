using IdentityService.Configuration;
using IdentityService.Data;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace IdentityService.Services
{
    public class IdentityInitializationService
    {
        /// <summary>
        /// Determines whether the specified application database context is initialized.
        /// </summary>
        /// <param name="_applicationDbContext">The application database context.</param>
        /// <returns>
        ///   <c>true</c> if the specified application database context is initialized; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInitialized(ApplicationDbContext _applicationDbContext)
        {
            var userCount = _applicationDbContext.Users.Count();
            var roleCount = _applicationDbContext.Roles.Count();

            return 0 < userCount && 3 == roleCount;
        }

        /// <summary>
        /// Initializes the specified user manager.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="roleManager">The role manager.</param>
        /// <returns></returns>
        public static async Task Initialize(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await EnsureRole(roleManager, IdentitySettings.GlobalAdministrator);
            await EnsureRole(roleManager, IdentitySettings.Administrator);
            await EnsureRole(roleManager, IdentitySettings.Customer);
            // Ensure root user
            var user = new IdentityUser { Email = IdentitySettings.RootUserName, UserName = IdentitySettings.RootUserName };
            var identityResult = await userManager.CreateAsync(user, "Pass.word1");
            ThrowIfFailedIdentityResult(identityResult);

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            identityResult = await userManager.ConfirmEmailAsync(user, token);
            ThrowIfFailedIdentityResult(identityResult);

            identityResult = await userManager.AddToRoleAsync(user, IdentitySettings.GlobalAdministrator);
            ThrowIfFailedIdentityResult(identityResult);
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
