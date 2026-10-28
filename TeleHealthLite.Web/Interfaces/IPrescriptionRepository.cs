
using TeleHealthLite.Web.Models;

namespace TeleHealthLite.Web.Interfaces
{
    public interface IPrescriptionRepository
    {
        Task<List<Prescription>> GetAllAsync();

        Task AddAsync(Prescription prescription);

        Task<List<Medicine>> SearchMedicinesAsync(string term);

        Task<List<Consultation>> GetAvailableConsultationsAsync();
    }
}
