namespace ELTBackend.DTOs
{
    public class LeaveReadDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = String.Empty;
        public Guid UserId { get; set; }
    }
}
