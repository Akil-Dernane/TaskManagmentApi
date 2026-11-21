using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.ExceptionHandling
{
    public class AuthenticationExceptionHandler : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService;

        public AuthenticationExceptionHandler(
            IProblemDetailsService problemDetailsService)
        {
            _problemDetailsService = problemDetailsService;
        }
       
        public async ValueTask<bool> TryHandleAsync(
            HttpContext context,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var (statusCode, title) = exception switch
            {
                UnauthorizedException =>
                    (StatusCodes.Status401Unauthorized, "Unauthorized"),

                InvalidRefreshTokenException => 
                    (StatusCodes.Status401Unauthorized, "Invalid refresh token"),

                RefreshTokenReuseException => 
                    (StatusCodes.Status401Unauthorized, "Refresh token reuse detected"),

                ForbiddenAccessException =>
                    (StatusCodes.Status403Forbidden, "Forbidden"),

                AccountLockedException =>
                    (StatusCodes.Status423Locked, "Account Locked"),

                ForbiddenException =>
                    (StatusCodes.Status403Forbidden, "Forbidden"),

                _ => (0, string.Empty)
            };

            if (statusCode == 0)
                return false;

            context.Response.StatusCode = statusCode;

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message,
                Instance = context.Request.Path
            };

            if (exception is Application.Exceptions.ApplicationException appException)
            {
                problemDetails.Extensions["errorCode"] = appException.ErrorCode;
            }

            problemDetails.Extensions["traceId"] = context.TraceIdentifier;

            await _problemDetailsService.WriteAsync(
                new ProblemDetailsContext
                {
                    HttpContext = context,
                    ProblemDetails = problemDetails
                });

            return true;
        }
    }
}