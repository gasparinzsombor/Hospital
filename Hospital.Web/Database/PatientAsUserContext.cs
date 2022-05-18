using Hospital.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Database
{
    public class PatientAsUserContext : IdentityDbContext<Patient, IdentityRole<int>, int>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Patient>().ToTable("Patients");
        }
        public DbSet<Doctor> Doctors { get; set; } = null!;
        public DbSet<Patient> Patients => Users;
        public DbSet<Specialization> Specializations { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;
        
        public DbSet<MedicalRecord> MedicalRecords { get; set; } = null!;
        
        public DbSet<Treatment> Treatments { get; set; } = null!;

        public PatientAsUserContext(DbContextOptions<PatientAsUserContext> options) : base(options) { }
    }
}
