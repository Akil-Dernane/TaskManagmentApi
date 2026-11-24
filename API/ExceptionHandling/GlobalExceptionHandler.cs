using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.ExceptionHandling;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (status, title, detail) = exception switch
        {
            ConflictException =>
                (409, "Conflict", exception.Message),

            ArgumentException =>
                (400, "Validation Error", exception.Message),

            InvalidOperationException =>
                (409, "Business Rule Violation", exception.Message),

            KeyNotFoundException =>
                (404, "Not Found", exception.Message),

            IdentityCreationException identityException =>
                (400, "Identity Validation Error",
                    string.Join(" ", identityException.Errors)),

            _ =>
                (500, "Internal Server Error",
                    "An unexpected error occurred.")
        };

        if (status == 500)
            logger.LogError(exception, "Unhandled exception.");
        else
            logger.LogWarning(exception, "Request failed: {Message}", exception.Message);

        context.Response.StatusCode = status;

        var problemDetails = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        if (exception is Application.Exceptions.ApplicationException appException)
        {
            problemDetails.Extensions["errorCode"] = appException.ErrorCode;
        }

        problemDetails.Extensions["traceId"] = context.TraceIdentifier;

        await problemDetailsService.WriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = context,
                ProblemDetails = problemDetails
            });

        return true;
    }
}