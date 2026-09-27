namespace MediVora.Common
{
    public sealed class ApiResponse<T>
    {
        public bool Success { get; init; }
        public string Message { get; init; } = string.Empty;
        public T? Data { get; init; }
        public IReadOnlyCollection<string>? Errors { get; init; }

        public static ApiResponse<T> SuccessResponse(
            T data,
            string message,
            string? traceId = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Errors = null,
            };
        }

        public static ApiResponse<T> ErrorResponse(
            string message,
            IReadOnlyCollection<string>? errors = null,
            string? traceId = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
                Errors = errors,
            };
        }
    }
}