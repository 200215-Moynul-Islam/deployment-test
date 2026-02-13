using ELTBackend.Models;

namespace ELTBackend.DTOs
{
    public class LeaveReadDetailsDto : LeaveReadDto
    {
        public UserReadDto User { get; set; } = null!;
    }
}
