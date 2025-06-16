using EMS.BLL.DTOs.Request;
using FluentValidation;

namespace EMS.BLL.DTOs.Validation;

public class OrganizerCreateRequestDTO_Validation : AbstractValidator<OrganizerCreateRequestDTO>
{
    public OrganizerCreateRequestDTO_Validation()
    {
        RuleFor(model => model.Name).NotNull().WithMessage("Name cannot be null");
        RuleFor(model => model.Email).NotNull().WithMessage("Email cannot be null");
    }
}