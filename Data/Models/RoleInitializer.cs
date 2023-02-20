using FurnitureStore.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FurnitureStore.Data.Models
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string password = "Qwerty123!";
            if (await roleManager.FindByNameAsync("Администратор") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Администратор"));
            }
            if (await roleManager.FindByNameAsync("Пользователь") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Пользователь"));
            }
            if (await roleManager.FindByNameAsync("Модератор") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Модератор"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Администратор");
                }
            }
        }
    }
}
