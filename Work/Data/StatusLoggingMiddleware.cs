using Microsoft.AspNetCore.Http.Features;
using Work.Controllers;

namespace Work.Data
{
    public class StatusLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<StatusLoggingMiddleware> _logger;

        public StatusLoggingMiddleware(RequestDelegate next, ILogger<StatusLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // Capture the response before it's sent to the client
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            // Log only if the status code is not in the 200 range
            if (context.Response.StatusCode == 400)
            {
                _logger.LogWarning("Status Code {StatusCode}: Invalid data received.", context.Response.StatusCode);
            }
            else if (context.Response.StatusCode == 404)
            {
                _logger.LogError("HTTP {StatusCode}: Page not found.", context.Response.StatusCode);
            }
            else if (context.Response.StatusCode == 500)
            {
                _logger.LogError("HTTP {StatusCode}: Internal server error.", context.Response.StatusCode);
            }

            // Copy the response body to the original stream and let the response continue
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
