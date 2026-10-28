
using TeleHealthLite.Web.Models;

namespace TeleHealthLite.Web.Interfaces
{
    public interface IConsultationService
    {
        Task<List<Consultation>> GetAllAsync();

        Task<Consultation?> GetByIdAsync(int id);

        Task AddAsync(Consultation consultation);

        Task<bool> PatientExistsAsync(int patientId);

        Task<bool> DoctorExistsAsync(int doctorId);
        Task<List<Doctor>> GetAllDoctorsAsync();
    }
}
