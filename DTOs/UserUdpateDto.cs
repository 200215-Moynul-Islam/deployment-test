using System.ComponentModel.DataAnnotations;
using ELTBackend.Constants;

namespace ELTBackend.DTOs
{
    public class UserUpdateDto
    {
        private string? _email;

        [Required(ErrorMessage = ErrorMessages.User.EmailRequired)]
        [MaxLength(
            ValidationConstants.User.MaxEmailLength,
            ErrorMessage = ErrorMessages.User.EmailMaxLengthExceeded
        )]
        [EmailAddress(ErrorMessage = ErrorMessages.User.InvalidEmailFormat)]
        public string? Email
        {
            get => _email;
            set => _email = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private string? _name;

        [Required(ErrorMessage = ErrorMessages.User.NameRequired)]
        [MaxLength(
            ValidationConstants.User.MaxNameLength,
            ErrorMessage = ErrorMessages.User.NameMaxLengthExceeded
        )]
        [RegularExpression(
            ValidationConstants.User.NameRegex,
            ErrorMessage = ErrorMessages.User.InvalidNameFormat
        )]
        public string? Name
        {
            get => _name;
            set => _name = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}
