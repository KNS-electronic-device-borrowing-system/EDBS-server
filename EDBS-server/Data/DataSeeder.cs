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

            // ==========================================
            // 1. KHỞI TẠO ROLES
            // ==========================================
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

            // ==========================================
            // 2. KHỞI TẠO USERS
            // ==========================================
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
                            Email = "user1@edbs.local",
                            Password = defaultPasswordHash,
                            FullName = "Sinh Viên Mượn Đồ",
                            RoleId = borrowerRole.Id,
                            IsVerified = true 
                        },
                        new User
                        {
                            Username = "admin1",
                            Email = "admin1@edbs.local",
                            Password = defaultPasswordHash,
                            FullName = "Quản Trị Viên Kho",
                            RoleId = adminRole.Id,
                            IsVerified = true
                        }
                    };
                    await context.Users.AddRangeAsync(users);
                    await context.SaveChangesAsync();
                }
            }

            // Lấy ID của Admin để gán cho cột CreatedBy của các thiết bị
            var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Username == "admin1");
            int adminId = adminUser?.Id ?? 1;

            // ==========================================
            // 3. KHỞI TẠO DANH MỤC (CATEGORIES)
            // ==========================================
            if (!await context.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category { CategoryName = "Vi điều khiển (Microcontrollers)", Description = "Các board mạch lập trình", CreatedBy = adminId },
                    new Category { CategoryName = "Cảm biến (Sensors)", Description = "Cảm biến nhiệt độ, siêu âm, ánh sáng...", CreatedBy = adminId },
                    new Category { CategoryName = "Thiết bị mạng", Description = "Router, Switch, Cáp mạng", CreatedBy = adminId }
                };
                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            // ==========================================
            // 4. KHỞI TẠO MODELS (Dòng sản phẩm)
            // ==========================================
            if (!await context.Models.AnyAsync())
            {
                var models = new List<Model>
                {
                    new Model { ModelName = "Arduino Uno R3", CreatedBy = adminId },
                    new Model { ModelName = "Raspberry Pi 4 Model B", CreatedBy = adminId },
                    new Model { ModelName = "Cisco Catalyst 2960", CreatedBy = adminId },
                    new Model { ModelName = "HC-SR04", CreatedBy = adminId } // Cảm biến siêu âm
                };
                await context.Models.AddRangeAsync(models);
                await context.SaveChangesAsync();
            }

            // ==========================================
            // 5. KHỞI TẠO PRODUCTS (Mẫu sản phẩm chung)
            // ==========================================
            if (!await context.Products.AnyAsync())
            {
                // Lấy data từ DB lên để map ID chuẩn xác
                var catMicro = await context.Categories.FirstOrDefaultAsync(c => c.CategoryName.Contains("Vi điều khiển"));
                var catNet = await context.Categories.FirstOrDefaultAsync(c => c.CategoryName.Contains("Thiết bị mạng"));
                var catSensor = await context.Categories.FirstOrDefaultAsync(c => c.CategoryName.Contains("Cảm biến"));

                var modArduino = await context.Models.FirstOrDefaultAsync(m => m.ModelName.Contains("Arduino"));
                var modPi = await context.Models.FirstOrDefaultAsync(m => m.ModelName.Contains("Raspberry"));
                var modCisco = await context.Models.FirstOrDefaultAsync(m => m.ModelName.Contains("Cisco"));
                var modHCSR04 = await context.Models.FirstOrDefaultAsync(m => m.ModelName.Contains("HC-SR04"));

                var products = new List<Product>
                {
                    new Product
                    {
                        ProductName = "Kit Arduino Uno R3 Kèm Cáp USB",
                        Description = "Board mạch Arduino Uno R3 chip cắm dùng để học tập lập trình nhúng cơ bản.",
                        CategoryId = catMicro?.Id,
                        ModelId = modArduino?.Id,
                        CreatedBy = adminId
                    },
                    new Product
                    {
                        ProductName = "Raspberry Pi 4 8GB RAM",
                        Description = "Máy tính nhúng nhỏ gọn cấu hình cao.",
                        CategoryId = catMicro?.Id,
                        ModelId = modPi?.Id,
                        CreatedBy = adminId
                    },
                    new Product
                    {
                        ProductName = "Switch Cisco Catalyst 2960 24 Port",
                        Description = "Thiết bị chuyển mạch mạng 24 cổng dùng cho thực hành mạng máy tính.",
                        CategoryId = catNet?.Id,
                        ModelId = modCisco?.Id,
                        CreatedBy = adminId
                    },
                    new Product
                    {
                        ProductName = "Cảm biến đo khoảng cách siêu âm HC-SR04",
                        Description = "Dùng để đo khoảng cách từ 2cm - 400cm.",
                        CategoryId = catSensor?.Id,
                        ModelId = modHCSR04?.Id,
                        CreatedBy = adminId
                    }
                };
                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }

            // ==========================================
            // 6. KHỞI TẠO ASSETS (Thiết bị vật lý cụ thể)
            // ==========================================
            if (!await context.Assets.AnyAsync())
            {
                var prodArduino = await context.Products.FirstOrDefaultAsync(p => p.ProductName.Contains("Arduino Uno R3"));
                var prodPi = await context.Products.FirstOrDefaultAsync(p => p.ProductName.Contains("Raspberry Pi 4"));
                var prodCisco = await context.Products.FirstOrDefaultAsync(p => p.ProductName.Contains("Cisco Catalyst 2960"));

                // Giả lập trong kho đang có 2 board Arduino, 1 Raspberry Pi và 1 Switch Cisco
                var assets = new List<Asset>
                {
                    new Asset
                    {
                        ProductId = prodArduino?.Id,
                        AssetTag = "ARD-001",
                        ManufacturerSerial = "SN-ARD-88392", 
                        Status = "READY", 
                        CreatedBy = adminId
                    },
                    new Asset
                    {
                        ProductId = prodArduino?.Id,
                        AssetTag = "ARD-002",
                        ManufacturerSerial = "SN-ARD-88393",
                        Status = "READY",
                        CreatedBy = adminId
                    },
                    new Asset
                    {
                        ProductId = prodPi?.Id,
                        AssetTag = "RPI-001",
                        ManufacturerSerial = "SN-RPI-44991A",
                        Status = "READY",
                        CreatedBy = adminId
                    },
                    new Asset
                    {
                        ProductId = prodCisco?.Id,
                        AssetTag = "NET-SW-001",
                        ManufacturerSerial = "FOC12345678",
                        Status = "MAINTENANCE", 
                        CreatedBy = adminId
                    }
                };
                await context.Assets.AddRangeAsync(assets);
                await context.SaveChangesAsync();
            }

           
        }
    }
}