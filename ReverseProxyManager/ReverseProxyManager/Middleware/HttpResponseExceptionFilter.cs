using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReverseProxyManager.Exceptions;
using Serilog;

namespace ReverseProxyManager.Middleware
{
    public class HttpResponseExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            Log.Error(exception, exception.Message, exception.StackTrace);

            var statusCode = StatusCodes.Status500InternalServerError;
            var responseMessage = exception.Message;

            if (exception.GetType() == typeof(AlreadyExistsException))
            {
                statusCode = StatusCodes.Status409Conflict;
            }

            if (exception.GetType() == typeof(NotFoundException))
            {
                statusCode = StatusCodes.Status404NotFound;
            }

            if (exception.GetType() == typeof(ArgumentOutOfRangeException))
            {
                statusCode = StatusCodes.Status400BadRequest;
            }

            if (exception.GetType() == typeof(UnauthorizedAccessException))
            {
                statusCode = StatusCodes.Status401Unauthorized;
            }

            context.Result = new ObjectResult(new { message = responseMessage })
            {
                StatusCode = statusCode
            };
        }
    }
}
