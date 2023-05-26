using DatingAppApi.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DatingAppApi.Data
{
    public class Seed
    {
        // We using a static method here so that we dont have to create a new instance of the seed class when using this classes method such as SeedUsers
        // We will be able to say Seed.SeedUsers instead of var seed = new Seed() and then call the the SeedUsers through instantiation
        public static async Task SeedUsers(DataContext context)
        {
            // 1st check to see if we have any users inside our database already, because we dont want to keep seeding data into our database if we have.
            if (await context.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            foreach( var user in users )
            {
                using var hmac = new HMACSHA512();

                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmac.Key;

                context.Users.Add(user);
            }

            await context.SaveChangesAsync();
        }
    }
}
