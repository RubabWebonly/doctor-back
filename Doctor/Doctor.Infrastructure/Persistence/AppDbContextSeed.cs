using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence
{
    public static class AppDbContextSeed
    {
        public static void Seed(this ModelBuilder builder)
        {
            var hasher = new PasswordHasher<User>();
            string hashedPassword = hasher.HashPassword(null, "admin123");

            builder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FullName = "Dr. Cavanşir Vahabov",
                    PhoneNumber = "+9940104149525",
                    Email = "rubabhuseynova013@gmail.com",
                    CreatedDate = DateTime.UtcNow,
                    PasswordHash = hashedPassword
                }
            );
        }
    }
}
