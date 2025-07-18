using FluentValidation;
using ReverseProxyManager.Requests;


namespace ReverseProxyManager.Validation
{
    public class LoginRequestValidator: AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            this.RuleFor(x => x.Name).NotEmpty().NotNull();
            this.RuleFor(x => x.Password).NotEmpty().NotNull();
        }
    }
}
