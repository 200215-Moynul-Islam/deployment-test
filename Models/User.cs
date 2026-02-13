using System.ComponentModel.DataAnnotations;
using ELTBackend.Constants;

namespace ELTBackend.Models
{
    public class User : EntityBase
    {
        [Required]
        [MaxLength(ValidationConstants.User.MaxNameLength)]
        public string Name { get; set; } = String.Empty;

        [Required]
        [MaxLength(ValidationConstants.User.MaxEmailLength)]
        public string Email { get; set; } = String.Empty;

        [Required]
        [MaxLength(ValidationConstants.User.MaxRoleLength)]
        public string Role { get; set; } = String.Empty;

        [Required]
        public string PasswordHash { get; set; } = String.Empty;

        // Navigation property
        public ICollection<Leave> Leaves { get; set; } = [];
    }
}
