
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TeleHealthLite.Web.Interfaces;
using TeleHealthLite.Web.Models;

namespace TeleHealthLite.Web.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionController(
            IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var prescriptions =
                await _prescriptionService.GetAllAsync();

            return View(prescriptions);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadConsultationsAsync();
            return View(new Prescription());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Prescription prescription,
            List<int>? medicineIds,
            List<string?>? dosages,
            List<string?>? frequencies,
            List<int>? durations)
        {
            // Validate that all medicine lists are provided and match.
            if (medicineIds == null ||
                dosages == null ||
                frequencies == null ||
                durations == null ||
                medicineIds.Count == 0 ||
                medicineIds.Count != dosages.Count ||
                medicineIds.Count != frequencies.Count ||
                medicineIds.Count != durations.Count)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Please select at least one medicine and provide all medicine details.");
            }

            if (!ModelState.IsValid)
            {
                await LoadConsultationsAsync();
                return View(prescription);
            }

            // Explicit null guard before accessing the lists.
            if (medicineIds == null ||
                dosages == null ||
                frequencies == null ||
                durations == null)
            {
                await LoadConsultationsAsync();
                return View(prescription);
            }

            prescription.PrescriptionMedicines ??=
                new List<PrescriptionMedicine>();

            for (int i = 0; i < medicineIds.Count; i++)
            {
                string dosage = dosages[i]?.Trim() ?? string.Empty;
                string frequency = frequencies[i]?.Trim() ?? string.Empty;
                int duration = durations[i];

                if (string.IsNullOrWhiteSpace(dosage) ||
                    string.IsNullOrWhiteSpace(frequency) ||
                    duration <= 0)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Enter a dosage, frequency, and valid duration for every medicine.");

                    await LoadConsultationsAsync();
                    return View(prescription);
                }

                prescription.PrescriptionMedicines.Add(
                    new PrescriptionMedicine
                    {
                        MedicineId = medicineIds[i],
                        Dosage = dosage,
                        Frequency = frequency,
                        DurationDays = duration
                    });
            }

            await _prescriptionService.AddAsync(prescription);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SearchMedicines(string? term)
        {
            term = term?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(term))
            {
                return Json(Array.Empty<object>());
            }

            var medicines =
                await _prescriptionService.SearchMedicinesAsync(term);

            var result = medicines.Select(m => new
            {
                id = m.Id,
                name = m.Name,
                strength = m.Strength,
                dosageForm = m.DosageForm
            });

            return Json(result);
        }

        private async Task LoadConsultationsAsync()
        {
            var consultations =
                await _prescriptionService
                    .GetAvailableConsultationsAsync();

            var items = consultations.Select(c => new
            {
                c.Id,
                Display =
                    (c.Patient?.FullName ?? "Unknown patient") + " - " +
                    (c.Doctor?.FullName ?? "Unknown doctor") + " - " +
                    c.AppointmentDate.ToString("dd-MM-yyyy HH:mm")
            });

            ViewBag.Consultations = new SelectList(
                items,
                "Id",
                "Display");
        }
    }
}
