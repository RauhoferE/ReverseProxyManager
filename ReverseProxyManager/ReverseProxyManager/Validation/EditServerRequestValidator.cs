using FluentValidation;
using ReverseProxyManager.Requests;

namespace ReverseProxyManager.Validation
{
    public class EditServerRequestValidator : AbstractValidator<EditServerRequest>
    {
        public EditServerRequestValidator()
        {
            this.RuleFor(x => x.Name).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            this.RuleFor(x => x.Target).NotEmpty().NotNull().MinimumLength(1).MaximumLength(250);
            this.RuleFor(x => x.TargetPort).GreaterThan(0).LessThan(65536);

            this.When(x => x.RedirectsToHttps, () =>
            {
                this.RuleFor(x => x.CertificateId).GreaterThanOrEqualTo(1)
                .WithMessage("When redirecting to https you need to assign a certificate.");
            });
            this.When(x => x.RedirectsToHttps, () =>
            {
                this.RuleFor(x => x.UsesHttp).Equal(false)
                    .WithMessage("If you redirect to HTTPS, you cannot use HTTP at the same time.");
            });
        }
    }
}
