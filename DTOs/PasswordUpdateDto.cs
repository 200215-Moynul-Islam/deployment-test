using System.ComponentModel.DataAnnotations;
using ELTBackend.Constants;

namespace ELTBackend.DTOs
{
    public class PasswordUpdateDto
    {
        [Required(ErrorMessage = ErrorMessages.User.PasswordRequired)]
        [StringLength(
            ValidationConstants.User.MaxPasswordLength,
            MinimumLength = ValidationConstants.User.MinPasswordLength,
            ErrorMessage = ErrorMessages.User.PasswordLengthOutOfRange
        )]
        [RegularExpression(
            ValidationConstants.User.PasswordRegex,
            ErrorMessage = ErrorMessages.User.InvalidPasswordFormat
        )]
        public string Password { get; set; } = String.Empty;
    }
}
