using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Data.Configuration;

namespace NEXUS.Data.Configuration;

public class DatabaseSeeder
{
    public static async Task SeedDataAsync(IServiceProvider serviceProvider, ILogger<DatabaseSeeder> logger)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        try
        {
            context.Database.Migrate();

            await IdentityConfiguration.RoleSeeder.SeedRolesAsync(roleManager, logger);
            await IdentityConfiguration.AdminSeeder.SeedAdminUserAsync(userManager, roleManager, logger);

            logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding database");
            throw;
        }
    }
}
