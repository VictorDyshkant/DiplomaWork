using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VideoHosting.Core.Middlewares
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogMiddleware> _logger;

        public LogMiddleware(RequestDelegate requestDelegate, ILogger<LogMiddleware> logger)
        {
            _next = requestDelegate;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (InvalidDataException ex)
            {
                if (ex.Message != null)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(ex.Message);
                    context.Response.Body.Write(bytes);
                    _logger.LogError(ex.Message, ex);
                }
                else
                {
                    _logger.LogError("Error InvalidData", ex);
                }
                context.Response.StatusCode = 400;
            }
            catch (UnauthorizedAccessException ex)
            {
                if (ex.Message != null)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(ex.Message);
                    context.Response.Body.Write(bytes);
                    _logger.LogError(ex.Message, ex);
                }

                context.Response.StatusCode = 403;
            }
            catch (Exception ex)
            {
                if (ex.Message != null)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(ex.Message);
                    context.Response.Body.Write(bytes);
                    _logger.LogCritical(ex.Message, ex);
                }
                else
                {
                    _logger.LogCritical("Fatal", ex);
                }
                context.Response.StatusCode = 501;
            }
        }
    }
}
