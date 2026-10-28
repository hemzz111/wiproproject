
using System.ComponentModel.DataAnnotations;

namespace TeleHealthLite.Web.Models
{
    public class Consultation : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        public Patient? Patient { get; set; }

        [Required]
        public int DoctorId { get; set; }

        public Doctor? Doctor { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [StringLength(500)]
        public string Reason { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = "Pending";

        public Prescription? Prescription { get; set; }

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (AppointmentDate <= DateTime.Now)
            {
                yield return new ValidationResult(
                    "Appointment date and time must be in the future.",
                    new[] { nameof(AppointmentDate) });
            }

            if (string.IsNullOrWhiteSpace(Reason))
            {
                yield return new ValidationResult(
                    "Please enter the reason for consultation.",
                    new[] { nameof(Reason) });
            }
        }
    }
}
