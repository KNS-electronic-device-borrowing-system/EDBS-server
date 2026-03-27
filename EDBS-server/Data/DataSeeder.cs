using EDBS_server.Models;
using Microsoft.EntityFrameworkCore;


namespace EDBS_server.Data 
{
    public static class DataSeeder
    {
        public static async Task SeedDataAsync(AssetManagementDbContext context)
        {
            // 1. Tự động chạy Migration nếu database chưa được cập nhật
            await context.Database.MigrateAsync();

            // 2. Seed bảng Roles
            if (!await context.Roles.AnyAsync())
            {
                var roles = new List<Role>
                {
                    new Role
                    {
                        Name = "Admin",
                        Description = "Quản trị viên hệ thống, có toàn quyền quản lý kho và phê duyệt"
                    },
                    new Role
                    {
                        Name = "Borrower",
                        Description = "Người mượn, có quyền xem tài sản rảnh và tạo phiếu mượn"
                    }
                };

                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync(); // Lưu để EF Core sinh ra Id
            }

            // 3. Seed bảng Users
            if (!await context.Users.AnyAsync())
            {
                // Lấy Id của các role vừa tạo ra
                var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
                var borrowerRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Borrower");

                // Lưu ý: Trong thực tế, PasswordHash phải được mã hóa bằng BCrypt hoặc PBKDF2. 
                // Ở đây dùng chuỗi giả lập để test luồng trước.
                var users = new List<User>
                {
                    new User
                    {
                        FullName = "Nguyễn Quản Trị",
                        Email = "admin@company.com",
                        Password = "hashed_password_123",
                        EmployeeCode = "EMP-ADMIN",
                        Phone = "0901234567",
                        RoleId = adminRole!.Id // Gán Role Admin
                    },
                    new User
                    {
                        FullName = "Trần Văn Mượn",
                        Email = "borrower@company.com",
                        Password = "hashed_password_123",
                        EmployeeCode = "EMP-001",
                        Phone = "0912345678",
                        RoleId = borrowerRole!.Id // Gán Role Borrower
                    },
                    new User
                    {
                        FullName = "Lê Thị B",
                        Email = "lethib@company.com",
                        Password = "hashed_password_123",
                        EmployeeCode = "EMP-002",
                        Phone = "0923456789",
                        RoleId = borrowerRole.Id // Gán Role Borrower
                    }
                };

                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
            }
        }
    }
}