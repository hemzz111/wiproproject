
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TeleHealthLite.Web.Interfaces;
using TeleHealthLite.Web.Models;

namespace TeleHealthLite.Web.Controllers
{
    public class ConsultationController : Controller
    {
        private readonly IConsultationService _consultationService;
        private readonly IPatientService _patientService;

        public ConsultationController(
            IConsultationService consultationService,
            IPatientService patientService)
        {
            _consultationService = consultationService;
            _patientService = patientService;
        }

        // Display all consultations
        public async Task<IActionResult> Index()
        {
            var consultations =
                await _consultationService.GetAllAsync();

            return View(consultations);
        }

        // Display consultation creation form
        public async Task<IActionResult> Create()
        {
            await LoadDropdownsAsync();
            return View();
        }

        // Save a new consultation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Consultation consultation)
        {
            if (consultation.AppointmentDate <= DateTime.Now)
            {
                ModelState.AddModelError(
                    nameof(consultation.AppointmentDate),
                    "Please select a future appointment date and time.");
            }

            var patientExists =
                await _patientService.GetByIdAsync(
                    consultation.PatientId) != null;

            var doctorExists =
                await _consultationService.DoctorExistsAsync(
                    consultation.DoctorId);

            if (!patientExists)
            {
                ModelState.AddModelError(
                    nameof(consultation.PatientId),
                    "Please select a valid patient.");
            }

            if (!doctorExists)
            {
                ModelState.AddModelError(
                    nameof(consultation.DoctorId),
                    "Please select a valid doctor.");
            }

            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return View(consultation);
            }

            consultation.Status = "Pending";

            await _consultationService.AddAsync(consultation);

            return RedirectToAction(nameof(Index));
        }

        // Display details for one consultation
        public async Task<IActionResult> Details(int id)
        {
            var consultation =
                await _consultationService.GetByIdAsync(id);

            if (consultation == null)
            {
                return NotFound();
            }

            return View(consultation);
        }

        // Load patient and doctor dropdowns
       private async Task LoadDropdownsAsync()
{
    var patients = await _patientService.GetAllAsync();
    var doctors = await _consultationService.GetAllDoctorsAsync();

    ViewBag.Patients = new SelectList(
        patients, "Id", "FullName");

    ViewBag.Doctors = new SelectList(
        doctors, "Id", "FullName");
}
    }
}
