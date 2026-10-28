
using Microsoft.EntityFrameworkCore;
using TeleHealthLite.Web.Data;
using TeleHealthLite.Web.Interfaces;
using TeleHealthLite.Web.Models;

namespace TeleHealthLite.Web.Repositories
{
    public class ConsultationRepository : IConsultationRepository
    {
        private readonly ApplicationDbContext _context;

        public ConsultationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Consultation>> GetAllAsync()
        {
            return await _context.Consultations
                .Include(c => c.Patient)
                .Include(c => c.Doctor)
                .OrderByDescending(c => c.AppointmentDate)
                .ToListAsync();
        }

        public async Task<Consultation?> GetByIdAsync(int id)
        {
            return await _context.Consultations
                .Include(c => c.Patient)
                .Include(c => c.Doctor)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Consultation consultation)
        {
            _context.Consultations.Add(consultation);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> PatientExistsAsync(int patientId)
        {
            return await _context.Patients
                .AnyAsync(p => p.Id == patientId);
        }

        public async Task<bool> DoctorExistsAsync(int doctorId)
        {
            return await _context.Doctors
                .AnyAsync(d => d.Id == doctorId);
        }

        public async Task<List<Doctor>> GetAllDoctorsAsync()
{
    return await _context.Doctors
        .OrderBy(d => d.FullName)
        .ToListAsync();
}
    }
}
