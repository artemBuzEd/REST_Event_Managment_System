using EMS.BLL.DTOs.Auth;
using FluentValidation;

namespace EMS.BLL.DTOs.Validation.Auth;

public class RegistrRequestDTO_Validation : AbstractValidator<RegistrRequestDTO>
{
    public RegistrRequestDTO_Validation()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required.")
            .MinimumLength(4).WithMessage("UserName must be at least 4 characters.");
        
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Valid email address is required.");
        
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.");
    }   
}