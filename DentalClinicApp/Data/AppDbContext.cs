using DentalClinicApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DentalClinicApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<PatientPhoto> PatientPhotos => Set<PatientPhoto>();

        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) 
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        public bool EnsureCreated()
        {
            return Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Photos)
                .WithOne(p => p.Patient)
                .HasForeignKey(p => p.PatientId);
        }
    }
}
