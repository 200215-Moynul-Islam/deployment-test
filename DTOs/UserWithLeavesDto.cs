namespace ELTBackend.DTOs
{
    public class UserWithLeavesDto : UserReadDto
    {
        public IEnumerable<LeaveReadDto> Leaves { get; set; } = [];
    }
}
