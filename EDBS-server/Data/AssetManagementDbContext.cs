using EDBS_server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EDBS_server.Data
{
    public class AssetManagementDbContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionDetail> TransactionDetails { get; set; }

        public AssetManagementDbContext(DbContextOptions<AssetManagementDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. GLOBAL QUERY FILTERS: Mặc định lọc bỏ các record đã xóa mềm
            modelBuilder.Entity<Role>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<User>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<Category>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<Asset>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<Transaction>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<TransactionDetail>().HasQueryFilter(e => e.DeletedAt == null);

            // 2. UNIQUE INDEXES có chứa Soft Delete Filter
            // Lưu ý: dùng "deleted_at" vì chúng ta sẽ cấu hình SnakeCase bên Program.cs
            modelBuilder.Entity<Role>().HasIndex(e => e.Name).IsUnique().HasFilter("deleted_at IS NULL");
            modelBuilder.Entity<Category>().HasIndex(e => e.Name).IsUnique().HasFilter("deleted_at IS NULL");
            modelBuilder.Entity<Asset>().HasIndex(e => e.AssetCode).IsUnique().HasFilter("deleted_at IS NULL");
            modelBuilder.Entity<User>().HasIndex(e => e.Email).IsUnique().HasFilter("deleted_at IS NULL");
            modelBuilder.Entity<User>().HasIndex(e => e.EmployeeCode).IsUnique().HasFilter("deleted_at IS NULL");

            // 3. CONFIGURE RELATIONSHIPS & DELETE BEHAVIORS
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role).WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Asset>()
                .HasOne(a => a.Category).WithMany(c => c.Assets)
                .HasForeignKey(a => a.CategoryId).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Borrower).WithMany(u => u.BorrowedTransactions)
                .HasForeignKey(t => t.BorrowerId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Approver).WithMany(u => u.ApprovedTransactions)
                .HasForeignKey(t => t.ApprovedById).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TransactionDetail>()
                .HasOne(td => td.Transaction).WithMany(t => t.Details)
                .HasForeignKey(td => td.TransactionId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TransactionDetail>()
                .HasOne(td => td.Asset).WithMany(a => a.TransactionDetails)
                .HasForeignKey(td => td.AssetId).OnDelete(DeleteBehavior.Restrict);
        }

        // 4. OVERRIDE SAVECHANGES ĐỂ XỬ LÝ SOFT DELETE & AUDIT TIME TỰ ĐỘNG
        public override int SaveChanges()
        {
            HandleSoftDeleteAndAudit();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            HandleSoftDeleteAndAudit();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void HandleSoftDeleteAndAudit()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    // Chặn lệnh DELETE, chuyển thành UPDATE deleted_at
                    entry.State = EntityState.Modified;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
