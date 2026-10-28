
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace TeleHealthLite.Web.Filters
{
    public class LoggingActionFilter : IActionFilter
    {
        private readonly ILogger<LoggingActionFilter> _logger;

        public LoggingActionFilter(
            ILogger<LoggingActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation(
                "Starting action: {Controller}/{Action}",
                context.RouteData.Values["controller"],
                context.RouteData.Values["action"]);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                _logger.LogError(
                    context.Exception,
                    "Exception occurred in action: {Controller}/{Action}",
                    context.RouteData.Values["controller"],
                    context.RouteData.Values["action"]);
            }
            else
            {
                _logger.LogInformation(
                    "Finished action: {Controller}/{Action}",
                    context.RouteData.Values["controller"],
                    context.RouteData.Values["action"]);
            }
        }
    }
}
