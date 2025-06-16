using EMS.BLL.DTOs.Request;

namespace EMS.BLL.DTOs.Validation;
using FluentValidation;

public class AttendeeCreateRequestDTO_Validation : AbstractValidator<AttendeeCreateRequestDTO>
{
    public AttendeeCreateRequestDTO_Validation()
    {
        RuleFor(model => model.FirstName)
            .NotNull().WithMessage("First name cannot be null");
        
        RuleFor(model => model.LastName)
            .NotNull().WithMessage("Last name cannot be null");
        
        RuleFor(model => model.Email)
            .NotNull().WithMessage("Email cannot be null");
    }
}