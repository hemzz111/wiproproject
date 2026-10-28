
using TeleHealthLite.Web.Models;

namespace TeleHealthLite.Web.Interfaces
{
    public interface IFollowUpRepository
    {
        Task<List<FollowUp>> GetAllAsync();

        Task<FollowUp?> GetByIdAsync(int id);

        Task AddAsync(FollowUp followUp);

        Task UpdateAsync(FollowUp followUp);

        Task<bool> PatientExistsAsync(int patientId);

        Task<List<Patient>> GetAllPatientsAsync();
    }
}
