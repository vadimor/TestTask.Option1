using Microsoft.AspNetCore.Mvc.Filters;

namespace TestTask.Option1.Filters
{
    // This class catches all exceptions that were not caught.

    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;

        public HttpGlobalExceptionFilter(ILogger<HttpGlobalExceptionFilter> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            _logger.LogError(
                new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);
        }
    }
}
