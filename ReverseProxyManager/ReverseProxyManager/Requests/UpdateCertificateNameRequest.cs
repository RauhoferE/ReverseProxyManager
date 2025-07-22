using System.ComponentModel.DataAnnotations;

namespace ReverseProxyManager.Requests
{
    public class UpdateCertificateNameRequest : IValidatableObject
    {
        public string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Name) || Name.Length < 1 || Name.Length > 100)
            {
                yield return new ValidationResult(
                    $"",
                    new[] { nameof(Name) });
            }
        }
    }
}
