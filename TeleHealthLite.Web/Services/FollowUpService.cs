
using TeleHealthLite.Web.Interfaces;
using TeleHealthLite.Web.Models;

namespace TeleHealthLite.Web.Services
{
    public class FollowUpService : IFollowUpService
    {
        private readonly IFollowUpRepository _followUpRepository;

        public FollowUpService(
            IFollowUpRepository followUpRepository)
        {
            _followUpRepository = followUpRepository;
        }

        public async Task<List<FollowUp>> GetAllAsync()
        {
            return await _followUpRepository.GetAllAsync();
        }

        public async Task<FollowUp?> GetByIdAsync(int id)
        {
            return await _followUpRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(FollowUp followUp)
        {
            await _followUpRepository.AddAsync(followUp);
        }

        public async Task UpdateAsync(FollowUp followUp)
        {
            await _followUpRepository.UpdateAsync(followUp);
        }

        public async Task<bool> PatientExistsAsync(int patientId)
        {
            return await _followUpRepository
                .PatientExistsAsync(patientId);
        }

        public async Task<List<Patient>> GetAllPatientsAsync()
        {
            return await _followUpRepository.GetAllPatientsAsync();
        }
    }
}
