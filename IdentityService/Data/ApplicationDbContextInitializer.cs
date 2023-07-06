using IdentityService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Data
{
    public class ApplicationDbContextInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
        }

        public static void IdentityDbContextInitializer(WebApplication app) {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    Initialize(services);

                    var applicationDbContext = services.GetRequiredService<ApplicationDbContext>();
                    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    if (!IdentityInitializationService.IsInitialized(applicationDbContext))
                    {
                        IdentityInitializationService.Initialize(userManager, roleManager)
                            .GetAwaiter()
                            .GetResult();
                    }
                }
                catch (Exception exception)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(exception, "Failed to seed database");
                }
            }
        }
    }
}
