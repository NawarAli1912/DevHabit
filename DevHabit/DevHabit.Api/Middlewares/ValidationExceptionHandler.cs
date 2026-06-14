using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DevHabit.Api.Middlewares;

public class ValidationExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
        {
            return false;
        }

        var problemDetailsContext = new ProblemDetailsContext
        {
            HttpContext = httpContext, 
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Detail = "One or  more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
            }
        };

        var groupedErrorMessages = validationException.
            Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                item => item.Key,
                item => item
                    .Select(e => e.ErrorMessage)
                    .ToArray());
        
        problemDetailsContext.ProblemDetails.Extensions.Add("errors", groupedErrorMessages);
        
        return await problemDetailsService.TryWriteAsync(problemDetailsContext);
    }
}
