using Microsoft.EntityFrameworkCore;
using CareBaseApi.Models;

namespace CareBaseApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Business> Business => Set<Business>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Consultation> Consultations => Set<Consultation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Business>(entity =>
            {
                entity.HasKey(b => b.BusinessId);
                entity.Property(b => b.Name).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.Password).IsRequired();

                entity.HasOne(u => u.Business)
                      .WithMany(b => b.Users)
                      .HasForeignKey(u => u.BusinessId);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(p => p.PatientId);
                entity.Property(p => p.Name).IsRequired();

                entity.HasOne(p => p.Business)
                      .WithMany(b => b.Patients)
                      .HasForeignKey(p => p.BusinessId);
            });

            modelBuilder.Entity<Consultation>(entity =>
            {
                entity.HasKey(c => c.ConsultationId);
                entity.Property(c => c.Date).IsRequired();

                entity.HasOne(c => c.Patient)
                      .WithMany(p => p.Consultations)
                      .HasForeignKey(c => c.PatientId);
            });
        }
    }
}
