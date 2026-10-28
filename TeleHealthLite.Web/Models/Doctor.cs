using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TeleHealthLite.Web.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Specialization { get; set; } = string.Empty;

        [Range(0, 10000)]
        [Precision(10, 2)]
        public decimal ConsultationFee { get; set; }

        public ICollection<Consultation> Consultations { get; set; }
            = new List<Consultation>();
    }
}
