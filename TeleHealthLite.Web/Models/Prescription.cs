
using System.ComponentModel.DataAnnotations;

namespace TeleHealthLite.Web.Models
{
    public class Prescription
    {
        public int Id { get; set; }

        [Required]
        public int ConsultationId { get; set; }

        public Consultation? Consultation { get; set; }

        [Required]
        public DateTime PrescribedDate { get; set; } = DateTime.Now;

        [StringLength(1000)]
        public string? Instructions { get; set; }

        public ICollection<PrescriptionMedicine> PrescriptionMedicines
        { get; set; } = new List<PrescriptionMedicine>();
    }
}
