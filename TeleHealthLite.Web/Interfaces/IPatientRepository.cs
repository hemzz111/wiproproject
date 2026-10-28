using TeleHealthLite.Web.Models;

namespace TeleHealthLite.Web.Interfaces
{
    public interface IPatientRepository
    {
        Task<List<Patient>> GetAllAsync();

        Task<Patient?> GetByIdAsync(int id);

        Task AddAsync(Patient patient);

        Task UpdateAsync(Patient patient);

        Task DeleteAsync(int id);

        Task<bool> EmailExistsAsync(string email);
    }
}