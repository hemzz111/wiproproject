
using System.ComponentModel.DataAnnotations;

namespace TeleHealthLite.Web.Models
{
    public class FollowUp
    {
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        public Patient? Patient { get; set; }

        [Required]
        public DateTime ReminderDate { get; set; }

        [Required]
        [StringLength(300)]
        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; } = false;
    }
}
