
using Microsoft.EntityFrameworkCore;
using TeleHealthLite.Web.Data;
using TeleHealthLite.Web.Interfaces;
using TeleHealthLite.Web.Models;

namespace TeleHealthLite.Web.Repositories
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly ApplicationDbContext _context;

        public PrescriptionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Prescription>> GetAllAsync()
        {
            return await _context.Prescriptions
                .Include(p => p.Consultation)
                    .ThenInclude(c => c!.Patient)
                .Include(p => p.PrescriptionMedicines)
                    .ThenInclude(pm => pm.Medicine)
                .ToListAsync();
        }

        public async Task AddAsync(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Medicine>> SearchMedicinesAsync(
            string term)
        {
            term = term?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(term))
            {
                return new List<Medicine>();
            }

            return await _context.Medicines
                .Where(m => m.Name.Contains(term))
                .OrderBy(m => m.Name)
                .Take(10)
                .ToListAsync();
        }

        public async Task<List<Consultation>>
            GetAvailableConsultationsAsync()
        {
            return await _context.Consultations
                .Include(c => c.Patient)
                .Include(c => c.Doctor)
                .Where(c =>
                    c.Status != "Cancelled" &&
                    c.Patient != null &&
                    c.Doctor != null &&
                    !_context.Prescriptions.Any(
                        p => p.ConsultationId == c.Id))
                .OrderByDescending(c => c.AppointmentDate)
                .ToListAsync();
        }
    }
}
