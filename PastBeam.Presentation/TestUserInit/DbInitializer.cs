using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PastBeam.Core.Library.Entities;

public static class DbInitializer
{
    public static async Task SeedTestUserAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        string testEmail = "testuser@example.com";
        string testPassword = "Test1234!";

        var existingUser = await userManager.FindByEmailAsync(testEmail);
        if (existingUser == null)
        {
            var testUser = new AppUser
            {
                UserName = testEmail,
                Email = testEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(testUser, testPassword);
            if (result.Succeeded)
            {
                Console.WriteLine("✅ Тестового користувача створено.");
            }
            else
            {
                Console.WriteLine("❌ Помилка при створенні користувача:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error.Description}");
                }
            }
        }
        else
        {
            Console.WriteLine("ℹ️ Тестовий користувач вже існує.");
        }
    }
}

