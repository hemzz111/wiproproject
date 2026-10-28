
using TeleHealthLite.Web.Interfaces;
using TeleHealthLite.Web.Models;

namespace TeleHealthLite.Web.Services
{
    public class ConsultationService : IConsultationService
    {
        private readonly IConsultationRepository _consultationRepository;

        public ConsultationService(
            IConsultationRepository consultationRepository)
        {
            _consultationRepository = consultationRepository;
        }

        public async Task<List<Consultation>> GetAllAsync()
        {
            return await _consultationRepository.GetAllAsync();
        }

        public async Task<Consultation?> GetByIdAsync(int id)
        {
            return await _consultationRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Consultation consultation)
        {
            await _consultationRepository.AddAsync(consultation);
        }

        public async Task<bool> PatientExistsAsync(int patientId)
        {
            return await _consultationRepository
                .PatientExistsAsync(patientId);
        }

        public async Task<bool> DoctorExistsAsync(int doctorId)
        {
            return await _consultationRepository
                .DoctorExistsAsync(doctorId);
        }
        public async Task<List<Doctor>> GetAllDoctorsAsync()
{
    return await _consultationRepository.GetAllDoctorsAsync();
}
    }
}
