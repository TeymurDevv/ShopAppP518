using FluentValidation;
using ShopAppP518.Apps.AdminApp.Dtos.UserDto;

namespace ShopAppP518.Apps.AdminApp.Validators.UserValidators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(s => s.UserName).NotEmpty().WithMessage("not empty")
              .MaximumLength(100).WithMessage("max is 100");
            RuleFor(s => s.Password).NotEmpty().WithMessage("not empty")
               .MinimumLength(8)
               .MaximumLength(100).WithMessage("max is 100");
        }
    }
}
