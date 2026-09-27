using MediVora.Common;
using Microsoft.AspNetCore.Diagnostics;

namespace MediVora.Exceptions
{
    public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var traceId = httpContext.TraceIdentifier;

            // Log complete exception details on the server
            logger.LogError(
                exception,
                "Unhandled exception occurred. TraceId: {TraceId}, Method: {Method}, Path: {Path}",
                traceId,
                httpContext.Request.Method,
                httpContext.Request.Path
            );

            var (statusCode, message, errors) = MapException(exception);

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            var response = ApiResponse<object>.ErrorResponse(
                message: message,
                errors: errors,
                traceId: traceId
            );

            await httpContext.Response.WriteAsJsonAsync(
                response,
                cancellationToken
            );

            return true;
        }

        private static (int StatusCode, string Message, IReadOnlyCollection<string>? Errors) MapException(Exception exception)
        {
            return exception switch
            {
                // ---------------------------------------------
                // Application Exception
                // ---------------------------------------------

                AppException appException =>
                (
                    (int)appException.StatusCode,
                    appException.Message,
                    GetApplicationErrors(appException)
                ),

                // ---------------------------------------------
                // Argument Exceptions
                // ---------------------------------------------

                ArgumentNullException =>
                (
                    StatusCodes.Status400BadRequest,
                    "Invalid request.",
                    new[]
                    {
                    "A required value was not provided."
                    }
                ),

                ArgumentException =>
                (
                    StatusCodes.Status400BadRequest,
                    "Invalid request.",
                    new[]
                    {
                    "One or more request values are invalid."
                    }
                ),

                // ---------------------------------------------
                // Authentication / Authorization
                // ---------------------------------------------

                UnauthorizedAccessException =>
                (
                    StatusCodes.Status401Unauthorized,
                    "Unauthorized.",
                    null
                ),

                // ---------------------------------------------
                // Resource Not Found
                // ---------------------------------------------

                KeyNotFoundException =>
                (
                    StatusCodes.Status404NotFound,
                    "Resource not found.",
                    null
                ),

                // ---------------------------------------------
                // Unexpected Exception
                // ---------------------------------------------

                _ =>
                (
                    StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred.",
                    null
                )
            };
        }

        private static IReadOnlyCollection<string>? GetApplicationErrors(
            AppException exception)
        {
            // ValidationException contains multiple errors
            if (exception is ValidationException validationException)
            {
                return validationException.Errors.SelectMany(x => x.Value).ToArray();
            }

            // For normal application exceptions,
            // the message itself is enough.
            return null;
        }
    }
}
