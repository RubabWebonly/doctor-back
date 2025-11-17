using Doctor.Domain.Entities;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Doctor.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
        }

        // ===================== DbSets =====================
        public DbSet<SystemSetting> SystemSettings { get; set; }
        public DbSet<AppointmentSlot> AppointmentSlots { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentService> AppointmentServices { get; set; } // ✅ yeni cədvəl
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<Diet> Diets { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<TreatmentFile> TreatmentFiles { get; set; }
        public DbSet<TreatmentDiet> TreatmentDiets { get; set; }
        public DbSet<TreatmentPrescription> TreatmentPrescriptions { get; set; }
        public DbSet<PatientDiet> PatientDiets { get; set; }
        public DbSet<PatientPrescription> PatientPrescriptions { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Medicine> Medicines { get; set; }

        public DbSet<Category> Categories { get; set; }

        // ===================== Configuration =====================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);
            });

            modelBuilder.Entity<Medicine>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);
            });

            // 🔹 Əlaqə: Appointment → AppointmentSlot
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.AppointmentSlot)
                .WithMany()
                .HasForeignKey(a => a.AppointmentSlotId)
                .OnDelete(DeleteBehavior.Restrict); // prevent cascading deletes

            // 🔹 Yeni əlaqə: Appointment ↔ Service (Many-to-Many)
            modelBuilder.Entity<AppointmentService>()
                .HasKey(x => new { x.AppointmentId, x.ServiceId }); // birləşmiş açar

            modelBuilder.Entity<AppointmentService>()
                .HasOne(x => x.Appointment)
                .WithMany(a => a.AppointmentServices)
                .HasForeignKey(x => x.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppointmentService>()
                .HasOne(x => x.Service)
                .WithMany(s => s.AppointmentServices)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Əgər seed metodu varsa, saxla
            modelBuilder.Seed();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(w =>
                w.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
    }
}
