using Microsoft.EntityFrameworkCore;
using CareBaseApi.Models;

namespace CareBaseApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Business> Businesses => Set<Business>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Business>(entity =>
            {
                entity.HasKey(b => b.BusinessId); // chave primária nomeada BusinessId
                entity.Property(b => b.Name).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId); // chave primária nomeada UserId
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.PasswordHash).IsRequired();

                // FK para Business
                entity.HasOne(u => u.Business)
                      .WithMany(b => b.Users)
                      .HasForeignKey(u => u.BusinessId);
            });
        }
    }
}
