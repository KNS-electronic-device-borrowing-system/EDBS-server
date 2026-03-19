using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EDBS_server.Models;

namespace EDBS_server.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<BorrowingSlip> BorrowingSlips { get; set; }
        public DbSet<Penalty> Penalties { get; set; }
        public DbSet<Blacklist> Blacklists { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure table names to use snake_case
            builder.Entity<Role>().ToTable("roles");
            builder.Entity<User>().ToTable("users");
            builder.Entity<Category>().ToTable("categories");
            builder.Entity<Model>().ToTable("models");
            builder.Entity<Product>().ToTable("products");
            builder.Entity<Asset>().ToTable("assets");
            builder.Entity<BorrowingSlip>().ToTable("borrowing_slips");
            builder.Entity<Penalty>().ToTable("penalties");
            builder.Entity<Blacklist>().ToTable("blacklist");

            // Configure column names to use snake_case
            ConfigureColumnNames(builder);

            // Configure relationships
            ConfigureRelationships(builder);

            // Configure indexes
            ConfigureIndexes(builder);
        }

        private void ConfigureColumnNames(ModelBuilder builder)
        {
            // Role columns
            builder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.RoleName).HasColumnName("role_name");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            // User columns
            builder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Username).HasColumnName("username");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.FullName).HasColumnName("full_name");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");
                entity.Property(e => e.AvatarUrl).HasColumnName("avatar_url");
                entity.Property(e => e.IdCardImageUrl).HasColumnName("id_card_image_url");
                entity.Property(e => e.RoleId).HasColumnName("role_id");
                entity.Property(e => e.IsLocked).HasColumnName("is_locked");
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            // Category columns
            builder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CategoryName).HasColumnName("category_name");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            // Model columns
            builder.Entity<Model>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ModelName).HasColumnName("model_name");
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            // Product columns
            builder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ProductName).HasColumnName("product_name");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.ImageUrl).HasColumnName("image_url");
                entity.Property(e => e.CategoryId).HasColumnName("category_id");
                entity.Property(e => e.ModelId).HasColumnName("model_id");
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            // Asset columns
            builder.Entity<Asset>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ProductId).HasColumnName("product_id");
                entity.Property(e => e.AssetTag).HasColumnName("asset_tag");
                entity.Property(e => e.ManufacturerSerial).HasColumnName("manufacturer_serial");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            // BorrowingSlip columns
            builder.Entity<BorrowingSlip>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.AssetId).HasColumnName("asset_id");
                entity.Property(e => e.BorrowDate).HasColumnName("borrow_date");
                entity.Property(e => e.DueDate).HasColumnName("due_date");
                entity.Property(e => e.ActualReturnDate).HasColumnName("actual_return_date");
                entity.Property(e => e.Reason).HasColumnName("reason");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.ProcessedBy).HasColumnName("processed_by");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            // Penalty columns
            builder.Entity<Penalty>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.BorrowingSlipId).HasColumnName("borrowing_slip_id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Amount).HasColumnName("amount").HasPrecision(12, 2);
                entity.Property(e => e.DaysOverdue).HasColumnName("days_overdue");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            // Blacklist columns
            builder.Entity<Blacklist>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Reason).HasColumnName("reason");
                entity.Property(e => e.BannedAt).HasColumnName("banned_at");
                entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });
        }

        private void ConfigureRelationships(ModelBuilder builder)
        {
            // User - Role relationship
            builder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.SetNull);

            // User self-referencing relationships for audit trail
            builder.Entity<User>()
                .HasOne(u => u.CreatedByUser)
                .WithMany(u => u.CreatedUsers)
                .HasForeignKey(u => u.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<User>()
                .HasOne(u => u.UpdatedByUser)
                .WithMany(u => u.UpdatedUsers)
                .HasForeignKey(u => u.UpdatedBy)
                .OnDelete(DeleteBehavior.SetNull);

            // Category relationships
            builder.Entity<Category>()
                .HasOne(c => c.CreatedByUser)
                .WithMany(u => u.CategoriesCreated)
                .HasForeignKey(c => c.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Category>()
                .HasOne(c => c.UpdatedByUser)
                .WithMany(u => u.CategoriesUpdated)
                .HasForeignKey(c => c.UpdatedBy)
                .OnDelete(DeleteBehavior.SetNull);

            // Model relationships
            builder.Entity<Model>()
                .HasOne(m => m.CreatedByUser)
                .WithMany(u => u.ModelsCreated)
                .HasForeignKey(m => m.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Model>()
                .HasOne(m => m.UpdatedByUser)
                .WithMany(u => u.ModelsUpdated)
                .HasForeignKey(m => m.UpdatedBy)
                .OnDelete(DeleteBehavior.SetNull);

            // Product relationships
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Product>()
                .HasOne(p => p.Model)
                .WithMany(m => m.Products)
                .HasForeignKey(p => p.ModelId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Product>()
                .HasOne(p => p.CreatedByUser)
                .WithMany(u => u.ProductsCreated)
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Product>()
                .HasOne(p => p.UpdatedByUser)
                .WithMany(u => u.ProductsUpdated)
                .HasForeignKey(p => p.UpdatedBy)
                .OnDelete(DeleteBehavior.SetNull);

            // Asset relationships
            builder.Entity<Asset>()
                .HasOne(a => a.Product)
                .WithMany(p => p.Assets)
                .HasForeignKey(a => a.ProductId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Asset>()
                .HasOne(a => a.CreatedByUser)
                .WithMany(u => u.AssetsCreated)
                .HasForeignKey(a => a.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Asset>()
                .HasOne(a => a.UpdatedByUser)
                .WithMany(u => u.AssetsUpdated)
                .HasForeignKey(a => a.UpdatedBy)
                .OnDelete(DeleteBehavior.SetNull);

            // BorrowingSlip relationships
            builder.Entity<BorrowingSlip>()
                .HasOne(bs => bs.User)
                .WithMany(u => u.BorrowingSlips)
                .HasForeignKey(bs => bs.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<BorrowingSlip>()
                .HasOne(bs => bs.Asset)
                .WithMany(a => a.BorrowingSlips)
                .HasForeignKey(bs => bs.AssetId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<BorrowingSlip>()
                .HasOne(bs => bs.ProcessedByUser)
                .WithMany(u => u.ProcessedBorrowingSlips)
                .HasForeignKey(bs => bs.ProcessedBy)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<BorrowingSlip>()
                .HasOne(bs => bs.CreatedByUser)
                .WithMany()
                .HasForeignKey(bs => bs.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<BorrowingSlip>()
                .HasOne(bs => bs.UpdatedByUser)
                .WithMany()
                .HasForeignKey(bs => bs.UpdatedBy)
                .OnDelete(DeleteBehavior.SetNull);

            // Penalty relationships
            builder.Entity<Penalty>()
                .HasOne(p => p.BorrowingSlip)
                .WithMany(bs => bs.Penalties)
                .HasForeignKey(p => p.BorrowingSlipId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Penalty>()
                .HasOne(p => p.User)
                .WithMany(u => u.Penalties)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Penalty>()
                .HasOne(p => p.CreatedByUser)
                .WithMany()
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Penalty>()
                .HasOne(p => p.UpdatedByUser)
                .WithMany()
                .HasForeignKey(p => p.UpdatedBy)
                .OnDelete(DeleteBehavior.SetNull);

            // Blacklist relationships
            builder.Entity<Blacklist>()
                .HasOne(b => b.User)
                .WithMany(u => u.Blacklists)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Blacklist>()
                .HasOne(b => b.CreatedByUser)
                .WithMany()
                .HasForeignKey(b => b.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Blacklist>()
                .HasOne(b => b.UpdatedByUser)
                .WithMany()
                .HasForeignKey(b => b.UpdatedBy)
                .OnDelete(DeleteBehavior.SetNull);
        }

        private void ConfigureIndexes(ModelBuilder builder)
        {
            // Unique index for manufacturer_serial (where is_deleted is false)
            builder.Entity<Asset>()
                .HasIndex(a => a.ManufacturerSerial)
                .HasName("idx_assets_serial_unique");

            // Unique index for username
            builder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Unique index for asset_tag
            builder.Entity<Asset>()
                .HasIndex(a => a.AssetTag)
                .IsUnique();

            // Unique index for role_name
            builder.Entity<Role>()
                .HasIndex(r => r.RoleName)
                .IsUnique();
        }
    }
}
