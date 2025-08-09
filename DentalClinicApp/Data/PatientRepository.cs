using DentalClinicApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DentalClinicApp.Data
{
    public class PatientRepository : IDisposable
    {
        private readonly AppDbContext _db;

        public PatientRepository(AppDbContext db) 
        {
            _db = db;
        }

        public async Task<bool> Connect()
        {
            return await Task.Run(() => _db.EnsureCreated());
        }

        public async Task<IEnumerable<Patient>> GetAllPatients()
        {
            return await _db.Patients.Include(p => p.Photos).ToListAsync();
        }

        public async Task<Patient?> GetPatientById(int id)
        {
            return await _db.Patients.Include(p => p.Photos).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddPatientAsync(Patient patient)
        {
            _db.Patients.Add(patient);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<PatientPhoto>> GetPhotosByPatientId(int patientId)
        {
            return await _db.PatientPhotos.Where(p => p.PatientId == patientId)
                             .OrderByDescending(p => p.UploadedAt)
                             .ToListAsync();
        }

        public async Task AddPhotoASync(PatientPhoto photo)
        {
            _db.PatientPhotos.Add(photo);
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
