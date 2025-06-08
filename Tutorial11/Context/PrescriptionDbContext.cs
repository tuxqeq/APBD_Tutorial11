using Tutorial11.Models;

namespace Tutorial11.Context
{
    using System;
    using Microsoft.EntityFrameworkCore;

    public class PrescriptionDbContext : DbContext
    {
        public PrescriptionDbContext(DbContextOptions<PrescriptionDbContext> options)
            : base(options) { }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionMedication> PrescriptionMedications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PrescriptionMedication>()
                .HasKey(pm => new { pm.PrescriptionId, pm.MedicationId });

            modelBuilder.Entity<PrescriptionMedication>()
                .HasOne(pm => pm.Prescription)
                .WithMany(p => p.PrescriptionMedications)
                .HasForeignKey(pm => pm.PrescriptionId);

            modelBuilder.Entity<PrescriptionMedication>()
                .HasOne(pm => pm.Medication)
                .WithMany(m => m.PrescriptionMedications)
                .HasForeignKey(pm => pm.MedicationId);

            modelBuilder.Entity<Doctor>().HasData(
                new Doctor { DoctorId = 1, FirstName = "John", LastName = "Doe", RowVersion = new byte[] {1,0,0,0,0,0,0,0} },
                new Doctor { DoctorId = 2, FirstName = "Jane", LastName = "Smith", RowVersion = new byte[] {2,0,0,0,0,0,0,0} }
            );

            modelBuilder.Entity<Patient>().HasData(
                new Patient { PatientId = 1, FirstName = "Alice", LastName = "Jones", RowVersion = new byte[] {3,0,0,0,0,0,0,0} }
            );

            modelBuilder.Entity<Medication>().HasData(
                new Medication { MedicationId = 1, Name = "Aspirin", RowVersion = new byte[] {4,0,0,0,0,0,0,0} },
                new Medication { MedicationId = 2, Name = "Ibuprofen", RowVersion = new byte[] {5,0,0,0,0,0,0,0} },
                new Medication { MedicationId = 3, Name = "Paracetamol", RowVersion = new byte[] {6,0,0,0,0,0,0,0} }
            );

            modelBuilder.Entity<Prescription>().HasData(
                new Prescription { PrescriptionId = 1, Date = new DateTime(2025, 1, 1), DueDate = new DateTime(2025, 1, 10), PatientId = 1, DoctorId = 1, RowVersion = new byte[] {7,0,0,0,0,0,0,0} },
                new Prescription { PrescriptionId = 2, Date = new DateTime(2025, 2, 1), DueDate = new DateTime(2025, 2, 5), PatientId = 1, DoctorId = 2, RowVersion = new byte[] {8,0,0,0,0,0,0,0} }
            );

            modelBuilder.Entity<PrescriptionMedication>().HasData(
                new { PrescriptionId = 1, MedicationId = 1, Dose = 2 },
                new { PrescriptionId = 1, MedicationId = 2, Dose = 1 },
                new { PrescriptionId = 2, MedicationId = 3, Dose = 5 }
            );
        }
    }
}