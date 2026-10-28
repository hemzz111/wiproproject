
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TeleHealthLite.Web.Interfaces;
using TeleHealthLite.Web.Models;

namespace TeleHealthLite.Web.Controllers
{
    public class FollowUpController : Controller
    {
        private readonly IFollowUpService _followUpService;

        public FollowUpController(IFollowUpService followUpService)
        {
            _followUpService = followUpService;
        }

        public async Task<IActionResult> Index()
        {
            var followUps = await _followUpService.GetAllAsync();
            return View(followUps);
        }

        public async Task<IActionResult> Create()
        {
            await LoadPatientsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FollowUp followUp)
        {
            if (followUp.ReminderDate < DateTime.Now)
            {
                ModelState.AddModelError(
                    nameof(followUp.ReminderDate),
                    "Please select a future follow-up date and time.");
            }

            if (!await _followUpService.PatientExistsAsync(
                    followUp.PatientId))
            {
                ModelState.AddModelError(
                    nameof(followUp.PatientId),
                    "Please select a valid patient.");
            }

            if (!ModelState.IsValid)
            {
                await LoadPatientsAsync();
                return View(followUp);
            }

            followUp.IsCompleted = false;
            await _followUpService.AddAsync(followUp);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkCompleted(int id)
        {
            var followUp = await _followUpService.GetByIdAsync(id);

            if (followUp == null)
            {
                return NotFound();
            }

            followUp.IsCompleted = true;
            await _followUpService.UpdateAsync(followUp);

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadPatientsAsync()
        {
            var patients = await _followUpService.GetAllPatientsAsync();

            ViewBag.Patients = new SelectList(
                patients,
                "Id",
                "FullName");
        }
    }
}
