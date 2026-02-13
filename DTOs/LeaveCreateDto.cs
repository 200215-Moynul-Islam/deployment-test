using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using ELTBackend.Constants;

namespace ELTBackend.DTOs
{
    public class LeaveCreateDto : IValidatableObject
    {
        [Required(ErrorMessage = ErrorMessages.Leave.StartDateRequired)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = ErrorMessages.Leave.EndDateRequired)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = ErrorMessages.Leave.UserIdRequired)]
        public Guid UserId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate < StartDate)
            {
                yield return new ValidationResult(
                    ErrorMessages.Leave.EndDateMustBeOnOrAfterStartDate,
                    new[] { nameof(EndDate) }
                );
            }
        }
    }
}
