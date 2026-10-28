
using TeleHealthLite.Web.Models;

namespace TeleHealthLite.Web.Events
{
    // Delegate defines the signature for prescription notifications.
    public delegate void PrescriptionCreatedHandler(
        object? sender,
        Prescription prescription);

    public class PrescriptionCreatedEvent
    {
        // Event allows subscribers to respond to prescription creation.
        public event PrescriptionCreatedHandler? PrescriptionCreated;

        public void Notify(Prescription prescription)
        {
            PrescriptionCreated?.Invoke(this, prescription);
        }
    }
}
