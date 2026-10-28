
using System.ComponentModel.DataAnnotations;

namespace TeleHealthLite.Web.Models
{
    public class Medicine
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Strength { get; set; }

        [StringLength(100)]
        public string? DosageForm { get; set; }

        public ICollection<PrescriptionMedicine> PrescriptionMedicines
        { get; set; } = new List<PrescriptionMedicine>();
    }
}
