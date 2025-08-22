using Microsoft.EntityFrameworkCore;
using CareBaseApi.Models;
using CareBaseApi.Enums;

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
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<PaymentInstallment> PaymentInstallments => Set<PaymentInstallment>(); // 👈 nova tabela

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🔹 Business
            modelBuilder.Entity<Business>(entity =>
            {
                entity.HasKey(b => b.BusinessId);
                entity.Property(b => b.Name).IsRequired();
            });

            // 🔹 User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.Password).IsRequired();

                entity.HasOne(u => u.Business)
                      .WithMany(b => b.Users)
                      .HasForeignKey(u => u.BusinessId);
            });

            // 🔹 Patient
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(p => p.PatientId);
                entity.Property(p => p.Name).IsRequired();

                entity.HasOne(p => p.Business)
                      .WithMany(b => b.Patients)
                      .HasForeignKey(p => p.BusinessId);
            });

            // 🔹 Consultation
            modelBuilder.Entity<Consultation>(entity =>
            {
                entity.HasKey(c => c.ConsultationId);

                entity.Property(c => c.StartDate).IsRequired();
                entity.Property(c => c.EndDate).IsRequired(false);

                entity.HasOne(c => c.Patient)
                      .WithMany(p => p.Consultations)
                      .HasForeignKey(c => c.PatientId);

                // relação com pagamentos
                entity.HasMany(c => c.Payments)
                      .WithOne(p => p.Consultation)
                      .HasForeignKey(p => p.ConsultationId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 🔹 ConsultationDetails
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

            // 🔹 Payment
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.PaymentId);

                entity.Property(p => p.Amount)
                      .HasColumnType("numeric(12,2)")
                      .IsRequired();

                entity.Property(p => p.Installments)
                      .HasDefaultValue(1)
                      .IsRequired();

                // enums como int (padrão)
                entity.Property(p => p.Method).IsRequired();
                entity.Property(p => p.Status)
                      .HasDefaultValue(PaymentStatus.Paid)
                      .IsRequired();

                entity.Property(p => p.PaidAt).IsRequired(false);
                entity.Property(p => p.ReferenceId).IsRequired(false);
                entity.Property(p => p.Notes).IsRequired(false);

                entity.HasIndex(p => p.ConsultationId);
            });

            // 🔹 PaymentInstallment
            modelBuilder.Entity<PaymentInstallment>(entity =>
            {
                entity.HasKey(pi => pi.PaymentInstallmentId);

                entity.Property(pi => pi.Amount)
                      .HasColumnType("numeric(12,2)")
                      .IsRequired();

                entity.Property(pi => pi.IsPaid)
                      .HasDefaultValue(false);

                entity.HasOne(pi => pi.Payment)
                      .WithMany(p => p.InstallmentsList)
                      .HasForeignKey(pi => pi.PaymentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(pi => pi.PaymentId);
            });

            // 👇 força todos os DateTime/DateTime? a "timestamp without time zone"
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
