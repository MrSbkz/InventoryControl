using InventoryControl.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace InventoryControl.Data.Initializers
{
    public static class RoleInitializer
    {
        public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync(Role.Admin) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Role.Admin));
            }

            if (await roleManager.FindByNameAsync(Role.Employee) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Role.Employee));
            }

            if (await roleManager.FindByNameAsync(Role.Accountant) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Role.Accountant));
            }
        }
    }
}
