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
        public DbSet<ConsultationDetails> ConsultationDetails => Set<ConsultationDetails>();


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

                entity.Property(c => c.StartDate)
                      .IsRequired(); // StartDate obrigatório

                entity.Property(c => c.EndDate)
                      .IsRequired(false); // EndDate opcional

                entity.HasOne(c => c.Patient)
                      .WithMany(p => p.Consultations)
                      .HasForeignKey(c => c.PatientId);
            });

            modelBuilder.Entity<ConsultationDetails>(entity =>
            {
                entity.HasKey(cd => cd.ConsultationDetailsId);

                entity.Property(cd => cd.Titulo1).IsRequired(false);
                entity.Property(cd => cd.Titulo2).IsRequired(false);
                entity.Property(cd => cd.Titulo3).IsRequired(false);
                entity.Property(cd => cd.Texto1).IsRequired(false);
                entity.Property(cd => cd.Texto2).IsRequired(false);
                entity.Property(cd => cd.Texto3).IsRequired(false);

                entity.HasOne(cd => cd.Consultation)
                      .WithOne(c => c.Details)
                      .HasForeignKey<ConsultationDetails>(cd => cd.ConsultationId)
                      .OnDelete(DeleteBehavior.Cascade);
            });



            // 👇 Força todos os DateTime/DateTime? a serem "timestamp without time zone"
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetColumnType("timestamp without time zone");
                    }
                }

            }
        }
    }
}
