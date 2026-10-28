
using Microsoft.EntityFrameworkCore;
using TeleHealthLite.Web.Data;
using TeleHealthLite.Web.Interfaces;
using TeleHealthLite.Web.Models;

namespace TeleHealthLite.Web.Repositories
{
    public class FollowUpRepository : IFollowUpRepository
    {
        private readonly ApplicationDbContext _context;

        public FollowUpRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FollowUp>> GetAllAsync()
        {
            return await _context.FollowUps
                .Include(f => f.Patient)
                .OrderBy(f => f.IsCompleted)
                .ThenBy(f => f.ReminderDate)
                .ToListAsync();
        }

        public async Task<FollowUp?> GetByIdAsync(int id)
        {
            return await _context.FollowUps
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task AddAsync(FollowUp followUp)
        {
            _context.FollowUps.Add(followUp);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FollowUp followUp)
        {
            _context.FollowUps.Update(followUp);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> PatientExistsAsync(int patientId)
        {
            return await _context.Patients
                .AnyAsync(p => p.Id == patientId);
        }

        public async Task<List<Patient>> GetAllPatientsAsync()
        {
            return await _context.Patients
                .OrderBy(p => p.FullName)
                .ToListAsync();
        }
    }
}
