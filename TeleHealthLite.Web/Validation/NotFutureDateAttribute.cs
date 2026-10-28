
using System.ComponentModel.DataAnnotations;

namespace TeleHealthLite.Web.Validation
{
    public class NotFutureDateAttribute : ValidationAttribute
    {
        public NotFutureDateAttribute()
        {
            ErrorMessage = "Date of birth cannot be in the future.";
        }

        public override bool IsValid(object? value)
        {
            if (value is DateTime date)
            {
                return date.Date <= DateTime.Today;
            }

            return true;
        }
    }
}
