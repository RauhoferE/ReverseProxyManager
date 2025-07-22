using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReverseProxyManager.Responses;

namespace ReverseProxyManager.Middleware
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errorsInModelState = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp=>kvp.Value.Errors.Select(e=>e.ErrorMessage)).ToArray();

                var errorResponse = new ErrorResponse();

                foreach (var error in errorsInModelState)
                {
                    foreach (var subError in error.Value)
                    {
                        var errorState = new ErrorState
                        {
                            FieldName = error.Key,
                            Message = subError
                        };
                    }
                }

                context.Result = new BadRequestObjectResult(errorResponse);
                return;
            }

            await next();
        }
    }
}
