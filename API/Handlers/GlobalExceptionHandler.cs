using Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.Handlers;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails;

        switch (exception)
        {
            // ---------------------------
            // 1) ValidationException
            // ---------------------------
            case ValidationException validationEx:
                _logger.LogWarning("Validation Exception Error: {Message}", validationEx.Message);

                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://tools.ietf.org/html/rfc7807",
                    Title = "Validation Exception Error",
                    Detail = validationEx.Message,
                };
                break;

            // ---------------------------
            // 2) BusinessException
            // ---------------------------
            case BusinessException businessEx:
                _logger.LogWarning("Business rule violation: {Message}", businessEx.Message);

                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Type = "https://example.com/errors/business-rule-violation",
                    Title = "Business Exception Error",
                    Detail = businessEx.Message,
                    Extensions =
                    {
                        ["errorCode"] = businessEx.ErrorCode
                    }
                };
                break;

            // ---------------------------
            // 3) Any unhandled exception
            // ---------------------------
            default:
                _logger.LogError(exception, "Unhandled exception occurred");

                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Type = "https://example.com/errors/internal-server-error",
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occurred. Please try again later."
                };
                break;
        }

        // Write response
        httpContext.Response.StatusCode = problemDetails.Status ?? 500;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true; // Exception handled
    }
}
