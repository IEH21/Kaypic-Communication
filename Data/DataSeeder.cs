using Microsoft.AspNetCore.Identity;
using web3_kaypic.Models;

public static class DataSeeder
{
    // Initialise des données en base
    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        foreach (var role in Enum.GetNames(typeof(Roles)))
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    /*
    public static async Task SeedAdminUser(UserManager<ApplicationUser> userManager)
    {
        //Email et mot de passe par défaut (à changer en prod)
        var adminEmail = "admin@gmail.com";
        var adminPassword = "Admintest123!";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Super",
                LastName = "Admin",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, Roles.Admin.ToString());
            }
        }
    }
    */
}
