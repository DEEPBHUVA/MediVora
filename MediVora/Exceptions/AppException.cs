using System.Net;

namespace MediVora.Exceptions
{
    public class AppException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : Exception(message)
    {
        public HttpStatusCode StatusCode { get; } = statusCode;
    }

    public sealed class NotFoundException(string resource) : AppException(
        $"{resource} was not found.",
        HttpStatusCode.NotFound);


    public sealed class ConflictException(string message) : AppException(message, HttpStatusCode.Conflict);

    public sealed class ValidationException(IDictionary<string, string[]> errors) : AppException(
            "One or more validation errors occurred.",
            HttpStatusCode.BadRequest)
    {
        public IReadOnlyDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>(errors);
    }
}
