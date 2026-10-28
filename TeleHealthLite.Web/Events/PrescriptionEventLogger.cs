
using Microsoft.Extensions.Logging;
using TeleHealthLite.Web.Models;

namespace TeleHealthLite.Web.Events
{
    public class PrescriptionEventLogger
    {
        private readonly ILogger<PrescriptionEventLogger> _logger;

        public PrescriptionEventLogger(
            ILogger<PrescriptionEventLogger> logger)
        {
            _logger = logger;
        }

        public void OnPrescriptionCreated(
            object? sender,
            Prescription prescription)
        {
            _logger.LogInformation(
                "Prescription created successfully. Prescription ID: {PrescriptionId}",
                prescription.Id);
        }
    }
}
