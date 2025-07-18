using FluentValidation;
using ReverseProxyManager.Requests;

namespace ReverseProxyManager.Validation
{
    public class UpdateCertificateNameRequestValidator: AbstractValidator<UpdateCertificateNameRequest>
    {
        public UpdateCertificateNameRequestValidator()
        {
            this.RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(100).MinimumLength(1);
        }
    }
}
