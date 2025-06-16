using EMS.BLL.DTOs.Request;
using FluentValidation;

namespace EMS.BLL.DTOs.Validation.UpdateValidation;

public class OrganizerUpdateRequestDTO_Validation : AbstractValidator<OrganizerUpdateRequestDTO>
{
    public OrganizerUpdateRequestDTO_Validation()
    {
        RuleFor(model => model.Name).NotNull().WithMessage("Name cannot be null");
        RuleFor(model => model.Email).NotNull().WithMessage("Email cannot be null");
    }
}