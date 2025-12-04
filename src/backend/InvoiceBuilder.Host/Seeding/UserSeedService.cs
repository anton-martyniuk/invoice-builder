using System.Security.Claims;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Modules.Users.Domain.Policies;
using Modules.Users.Domain.Users;
using Modules.Users.Infrastructure.Database;

namespace InvoiceBuilder.Host.Seeding;

public class UserSeedService(
    UsersDbContext usersContext,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    ILogger<UserSeedService> logger)
{
    public async Task SeedUsersAsync()
    {
	    Randomizer.Seed = new Random(4503);

        if (await usersContext.Users.AnyAsync())
        {
            logger.LogInformation("Users already exist, skipping user seeding");
            return;
        }

        logger.LogInformation("Starting user seeding...");

        await CreateRolesAsync();
        await CreateUsersAsync();

        await usersContext.SaveChangesAsync();

        logger.LogInformation("User seeding completed");
    }

    private async Task CreateRolesAsync()
    {
        var adminRole = new Role { Name = "Admin" };
        var managerRole = new Role { Name = "Manager" };

        await roleManager.CreateAsync(adminRole);
        await roleManager.CreateAsync(managerRole);

        await ConfigureAdminRolePermissions(adminRole);
    }

    private async Task ConfigureAdminRolePermissions(Role adminRole)
    {
        // Users module permissions
        await roleManager.AddClaimAsync(adminRole, new Claim(UserPolicyConsts.ReadPolicy, "true"));
        await roleManager.AddClaimAsync(adminRole, new Claim(UserPolicyConsts.CreatePolicy, "true"));
        await roleManager.AddClaimAsync(adminRole, new Claim(UserPolicyConsts.UpdatePolicy, "true"));
        await roleManager.AddClaimAsync(adminRole, new Claim(UserPolicyConsts.DeletePolicy, "true"));
    }

    private async Task CreateUsersAsync()
    {
        var adminUser = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = "admin@test.com",
            UserName = "admin@test.com"
        };

        await userManager.CreateAsync(adminUser, "Test1234!");
        await userManager.AddToRoleAsync(adminUser, "Admin");

        var managerUser = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = "manager@test.com",
            UserName = "manager@test.com"
        };

        await userManager.CreateAsync(managerUser, "Test1234!");
        await userManager.AddToRoleAsync(managerUser, "Manager");
    }
}
