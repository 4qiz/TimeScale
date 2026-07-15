using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ApplicationValidationException = TimeScale.Application.Exceptions.ValidationException;

namespace TimeScale.Api.Middlewares
{
    internal sealed class GlobalExceptionMiddleware(
        IProblemDetailsService problemDetailsService,
        ILogger<GlobalExceptionMiddleware> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            logger.LogError(exception, "Unhandled exception occurred");

            httpContext.Response.StatusCode = exception switch
            {
                ApplicationValidationException => StatusCodes.Status400BadRequest,
                FluentValidation.ValidationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            var isValidationFailure = exception is ApplicationValidationException
                || exception is FluentValidation.ValidationException;

            await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new ProblemDetails
                {
                    Type = exception.GetType().Name,
                    Title = isValidationFailure ? "Validation failed" : "An error occurred",
                    Detail = exception.Message
                }
            });

            return true;
        }
    }
}
