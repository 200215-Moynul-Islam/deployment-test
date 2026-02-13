namespace ELTBackend.DTOs
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
