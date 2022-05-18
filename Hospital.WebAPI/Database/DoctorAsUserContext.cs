using Hospital.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hospital.WebAPI.Database
{
    public class DoctorAsUserContext : IdentityDbContext<Doctor, IdentityRole<int>, int>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Doctor>().ToTable("Doctors");
        }

        public DbSet<Patient> Patients { get; set; } = null!;

        public DbSet<Doctor> Doctors => Users;

        public DbSet<Specialization> Specializations { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;
        public DbSet<MedicalRecord> MedicalRecords { get; set; } = null!;
        
        public DbSet<Treatment> Treatments { get; set; } = null!;

        public DoctorAsUserContext(DbContextOptions<DoctorAsUserContext> options) : base(options) { }
    }
}
