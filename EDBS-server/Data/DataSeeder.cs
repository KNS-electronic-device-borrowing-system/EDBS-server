using EDBS_server.Models;
using Microsoft.EntityFrameworkCore;

namespace EDBS_server.Data
{
    public static class DataSeeder
    {
        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // --- 1. KHỞI TẠO BẢNG ROLES ---
            if (!await context.Roles.AnyAsync())
            {
                var roles = new List<Role>
                {
                    new Role { RoleName = "Admin" },
                    new Role { RoleName = "Borrower" }
                };

                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();
            }

            // --- 2. KHỞI TẠO BẢNG USERS ---
          
            if (!await context.Users.AnyAsync())
            {
                var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Admin");
                var borrowerRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Borrower");

                if (adminRole != null && borrowerRole != null)
                {
                    string defaultPasswordHash = BCrypt.Net.BCrypt.HashPassword("1");

                    var users = new List<User>
                    {
                        new User
                        {
                            Username = "user1",
                            Password = defaultPasswordHash,
                            FullName = "Người Mượn Test",
                            RoleId = borrowerRole.Id,
                            IsLocked = false,
                            IsDeleted = false
                        },
                        new User
                        {
                            Username = "admin1",
                            Password = defaultPasswordHash,
                            FullName = "Quản Trị Viên",
                            RoleId = adminRole.Id, 
                            IsLocked = false,
                            IsDeleted = false
                        }
                    };

                    await context.Users.AddRangeAsync(users);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}