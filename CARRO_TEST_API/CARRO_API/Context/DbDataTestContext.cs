using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using CARRO_API.Entities;

namespace CARRO_API.Context
{
    public class DbDataTestContext : DbContext
    {
        public DbDataTestContext()
        {

        }

        public DbDataTestContext(DbContextOptions<DbDataTestContext> options) : base(options)
        {

        }

        protected DbDataTestContext(DbContextOptions options) : base(options)
        {

        }

        public DbConnection DbConnenct => Database.GetDbConnection();

        public void RefreshAll()
        {
            foreach (var entity in ChangeTracker.Entries())
            {
                entity.Reload();
            }
        }
        public virtual DbSet<User> Users { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Brithdate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.FirstName).HasMaxLength(250);

                entity.Property(e => e.LastName).HasMaxLength(250);

                entity.Property(e => e.ModifyBy).HasMaxLength(150);

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            });
        }
    }
}
