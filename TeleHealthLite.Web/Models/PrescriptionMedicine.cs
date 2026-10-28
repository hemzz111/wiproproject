
using System.ComponentModel.DataAnnotations;

namespace TeleHealthLite.Web.Models
{
    public class PrescriptionMedicine
    {
        public int Id { get; set; }

        [Required]
        public int PrescriptionId { get; set; }

        public Prescription? Prescription { get; set; }

        [Required]
        public int MedicineId { get; set; }

        public Medicine? Medicine { get; set; }

        [Required]
        [StringLength(100)]
        public string Dosage { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Frequency { get; set; } = string.Empty;

        [Range(1, 365)]
        public int DurationDays { get; set; }
    }
}
