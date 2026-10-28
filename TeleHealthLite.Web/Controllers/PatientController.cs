
using Microsoft.AspNetCore.Mvc;
using TeleHealthLite.Web.Interfaces;
using TeleHealthLite.Web.Models;
using System.Text;
using System.Linq;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace TeleHealthLite.Web.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // Display all patients
        public async Task<IActionResult> Index()
        {
            var patients = await _patientService.GetAllAsync();
            return View(patients);
        }

        // Display patient registration form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Save a new patient
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient patient)
        {
            if (!string.IsNullOrWhiteSpace(patient.Email) &&
                await _patientService.EmailExistsAsync(patient.Email))
            {
                ModelState.AddModelError(
                    nameof(patient.Email),
                    "This email address is already registered.");
            }

            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            await _patientService.AddAsync(patient);

            TempData["SuccessMessage"] =
                "Patient registered successfully.";

            return RedirectToAction(nameof(Index));
        }

        // Remote email validation
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Json(true);
            }

            bool emailExists =
                await _patientService.EmailExistsAsync(email);

            if (emailExists)
            {
                return Json(
                    "This email address is already registered.");
            }

            return Json(true);
        }

        // Display one patient
        public async Task<IActionResult> Details(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // Export patients to CSV
        public async Task<IActionResult> ExportCsv()
        {
            var patients = await _patientService.GetAllAsync();
            var csv = new StringBuilder();

            csv.AppendLine(
                "Id,FullName,Email,Phone,DateOfBirth");

            foreach (var patient in patients)
            {
                csv.AppendLine(
                    $"{patient.Id}," +
                    $"{EscapeCsv(patient.FullName)}," +
                    $"{EscapeCsv(patient.Email)}," +
                    $"{EscapeCsv(patient.Phone)}," +
                    $"{patient.DateOfBirth.ToString("yyyy-MM-dd")}");
            }

            byte[] bytes = Encoding.UTF8.GetPreamble()
                .Concat(Encoding.UTF8.GetBytes(csv.ToString()))
                .ToArray();

            return File(bytes, "text/csv", "Patients.csv");
        }

        // Import form
        [HttpGet]
        public IActionResult ImportCsv()
        {
            return View();
        }

        // Import patients from CSV
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportCsv(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                ModelState.AddModelError(
                    string.Empty, "Please select a CSV file.");
                return View();
            }

            if (!Path.GetExtension(csvFile.FileName)
                .Equals(".csv", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(
                    string.Empty, "Please upload a CSV file.");
                return View();
            }

            int importedCount = 0;
            int skippedCount = 0;

            using var reader = new StreamReader(
                csvFile.OpenReadStream(), Encoding.UTF8, true);

            // Skip the CSV header
            await reader.ReadLineAsync();

            string? line;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var fields = ParseCsvLine(line);

                if (fields == null || fields.Count != 5 ||
                    string.IsNullOrWhiteSpace(fields[1]) ||
                    string.IsNullOrWhiteSpace(fields[2]) ||
                    string.IsNullOrWhiteSpace(fields[3]) ||
                    !System.Net.Mail.MailAddress.TryCreate(
                        fields[2], out var address) ||
                    !string.Equals(
                        address.Address, fields[2],
                        StringComparison.OrdinalIgnoreCase) ||
                    !DateTime.TryParseExact(
                        fields[4], "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var dateOfBirth))
                {
                    skippedCount++;
                    continue;
                }

                // Prevent duplicate emails
                if (await _patientService.EmailExistsAsync(fields[2]))
                {
                    skippedCount++;
                    continue;
                }

                var patient = new Patient
                {
                    FullName = fields[1],
                    Email = fields[2],
                    Phone = fields[3],
                    DateOfBirth = dateOfBirth
                };

                // Validate the patient model
                ModelState.Clear();

                if (!TryValidateModel(patient))
                {
                    skippedCount++;
                    ModelState.Clear();
                    continue;
                }

                await _patientService.AddAsync(patient);
                importedCount++;
                ModelState.Clear();
            }

            TempData["SuccessMessage"] =
                $"CSV import completed. Imported: {importedCount}. " +
                $"Skipped: {skippedCount}.";

            return RedirectToAction(nameof(Index));
        }

        // Parse CSV values, including quoted commas and quotes
        private static List<string>? ParseCsvLine(string line)
        {
            var fields = new List<string>();
            var field = new StringBuilder();
            bool insideQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    if (insideQuotes &&
                        i + 1 < line.Length &&
                        line[i + 1] == '"')
                    {
                        field.Append('"');
                        i++;
                    }
                    else
                    {
                        insideQuotes = !insideQuotes;
                    }
                }
                else if (c == ',' && !insideQuotes)
                {
                    fields.Add(field.ToString());
                    field.Clear();
                }
                else
                {
                    field.Append(c);
                }
            }

            if (insideQuotes)
                return null;

            fields.Add(field.ToString());
            return fields;
        }

        // Escape CSV special characters
        private static string EscapeCsv(string? value)
        {
            value ??= string.Empty;
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }
    }
}
