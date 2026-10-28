
using TeleHealthLite.Web.Interfaces;
using TeleHealthLite.Web.Models;
using TeleHealthLite.Web.Events;

namespace TeleHealthLite.Web.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _prescriptionRepository;

        public event PrescriptionCreatedHandler? PrescriptionCreated;

        public PrescriptionService(
            IPrescriptionRepository prescriptionRepository)
        {
            _prescriptionRepository = prescriptionRepository;
        }

        public async Task<List<Prescription>> GetAllAsync()
        {
            return await _prescriptionRepository.GetAllAsync();
        }

        public async Task AddAsync(Prescription prescription)
        {
            // Save the prescription first.
            await _prescriptionRepository.AddAsync(prescription);

            // Notify subscribers only after saving succeeds.
            PrescriptionCreated?.Invoke(this, prescription);
        }

        public async Task<List<Medicine>> SearchMedicinesAsync(
            string term)
        {
            return await _prescriptionRepository
                .SearchMedicinesAsync(term);
        }

        public async Task<List<Consultation>>
            GetAvailableConsultationsAsync()
        {
            return await _prescriptionRepository
                .GetAvailableConsultationsAsync();
        }
    }
}
