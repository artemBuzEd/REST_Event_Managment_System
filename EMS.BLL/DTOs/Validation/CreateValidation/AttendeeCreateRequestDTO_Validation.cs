using EMS.BLL.DTOs.Request;

namespace EMS.BLL.DTOs.Validation;
using FluentValidation;

public class AttendeeCreateRequestDTO_Validation : AbstractValidator<AttendeeCreateRequestDTO>
{
    public AttendeeCreateRequestDTO_Validation()
    {
        RuleFor(model => model.FirstName)
            .NotNull().NotEmpty().WithMessage("First name cannot be null");
        
        RuleFor(model => model.LastName)
            .NotNull().NotEmpty().WithMessage("Last name cannot be null");
        
        RuleFor(model => model.Email)
            .NotNull().NotEmpty().WithMessage("Email cannot be null");
        RuleFor(model => model.Password)
            .NotNull().NotEmpty().WithMessage("Password cannot be null");
    }
}