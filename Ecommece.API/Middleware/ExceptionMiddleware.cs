using Ecommece.API.Errors;
using System.Net;
using System.Text.Json;

namespace Ecommece.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _enviroment;

        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger,IHostEnvironment environment)
        {
            _next = next;
            _logger=logger;
            _enviroment=environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                string message = e.Message;
                string details = _enviroment.IsDevelopment()
                    ? $"{e.StackTrace}\nInnerException: {e.InnerException?.Message}\n{e.InnerException?.StackTrace}"
                    : null;

                var response = new ApiExceptions(
                    (int)HttpStatusCode.InternalServerError,
                    message,
                    details
                );

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }

    }
}
