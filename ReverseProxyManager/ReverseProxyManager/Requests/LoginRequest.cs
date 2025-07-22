using System.ComponentModel.DataAnnotations;

namespace ReverseProxyManager.Requests
{
    public class LoginRequest : IValidatableObject
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Name))
            {
                yield return new ValidationResult(
                    $"",
                    new[] { nameof(Name) });
            }

            if (string.IsNullOrEmpty(Password))
            {
                yield return new ValidationResult(
                    $"",
                    new[] { nameof(Password) });
            }
        }
    }
}
