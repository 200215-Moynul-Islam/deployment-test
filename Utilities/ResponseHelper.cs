using ELTBackend.DTOs;

namespace ELTBackend.Utilities
{
    public static class ResponseHelper
    {
        public static ApiResponse Success(object? data = null, string? message = null)
        {
            return new ApiResponse
            {
                Success = true,
                Message = message,
                Data = data,
                Errors = null,
            };
        }

        public static ApiResponse Failure(
            string? message = null,
            IEnumerable<string>? errors = null
        )
        {
            return new ApiResponse
            {
                Success = false,
                Message = message,
                Data = null,
                Errors = errors,
            };
        }
    }
}
