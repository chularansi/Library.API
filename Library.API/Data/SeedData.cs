using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Data
{
    public static class SeedData
    {
        public async static Task Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // First create roles then users
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
        }

        private async static Task SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (await userManager.FindByNameAsync("admin") == null)
            {
                var user = new IdentityUser() { UserName = "admin@abc.no", Email = "admin@abc.no" };
                var result = await userManager.CreateAsync(user, "Password_123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Administrator");
                }
            }
            if (await userManager.FindByNameAsync("customer") == null)
            {
                var user = new IdentityUser() { UserName = "customer@abc.no", Email = "customer@abc.no" };
                var result = await userManager.CreateAsync(user, "Password_123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Customer");
                }
            }
        }

        private async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Administrator"))
            {
                var role = new IdentityRole() { Name = "Administrator" };
                await roleManager.CreateAsync(role);
            }
            
            if (!await roleManager.RoleExistsAsync("Customer"))
            {
                var role = new IdentityRole() { Name = "Customer" };
                await roleManager.CreateAsync(role);
            }
        }
    }
}
