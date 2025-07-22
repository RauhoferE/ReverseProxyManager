using FluentValidation;
using ReverseProxyManager.Requests;

namespace ReverseProxyManager.Validation
{
    public class EditServerRequestValidator : AbstractValidator<EditServerRequest>
    {
        public EditServerRequestValidator()
        {
            this.RuleFor(x => x.Name).NotEmpty().NotNull().MinimumLength(1).MaximumLength(100);
            this.RuleFor(x => x.Target).NotEmpty().NotNull().MinimumLength(1).MaximumLength(250).Must(x => IsValidHttpAddress(x));
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

        public static bool IsValidHttpAddress(string urlString)
        {
            // 1. Basic URI parsing:
            // UriKind.Absolute is crucial here. It requires the URL to have a scheme (like http:// or https://).
            // If the string is just "www.example.com", TryCreate with Absolute will return false.
            Uri uri = null;
            bool success = Uri.TryCreate(urlString, UriKind.Absolute, out uri);

            if (success)
            {
                // 2. Additional Scheme Check:
                // Ensure the scheme is specifically HTTP or HTTPS.
                // This prevents valid URIs like "ftp://", "mailto:", "file://", or "javascript:" from being considered valid HTTP addresses.
                if (uri!.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
                {
                    // 3. Optional: Additional Hostname/Port/Path checks if needed
                    // For instance, you might want to ensure the host is not empty,
                    // or if it's an IP address, that it's a valid IP.
                    // Uri.IsLoopback: Check if it's localhost or loopback IP
                    // Uri.IsFile: Check if it's a file path
                    // Uri.IsUnc: Check if it's a UNC path (network share)

                    // For a general "valid HTTP address", the scheme check is often sufficient
                    // as TryCreate already handles the general well-formedness.
                    return true;
                }
            }

            // If Uri.TryCreate failed, or if the scheme was not HTTP/HTTPS
            uri = null; // Explicitly set to null if validation failed
            return false;
        }
    }
}
