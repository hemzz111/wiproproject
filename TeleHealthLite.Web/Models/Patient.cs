using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TeleHealthLite.Web.Validation;

namespace TeleHealthLite.Web.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Remote(action: "VerifyEmail", controller: "Patient")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [NotFutureDate]
        public DateTime DateOfBirth { get; set; }

        public ICollection<Consultation> Consultations { get; set; }
            = new List<Consultation>();

        public ICollection<FollowUp> FollowUps { get; set; }
            = new List<FollowUp>();
    }
}
