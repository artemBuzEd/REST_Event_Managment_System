using EMS.BLL.DTOs.Auth;
using FluentValidation;

namespace EMS.BLL.DTOs.Validation.Auth;

public class LoginRequestDTO_Validation : AbstractValidator<LoginRequestDTO>
{
    public LoginRequestDTO_Validation()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(4).WithMessage("Username must be at least 4 characters.");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.");
    }
}