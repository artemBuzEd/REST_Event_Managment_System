using EMS.BLL.DTOs.Request.Attendee;
using FluentValidation;

namespace EMS.BLL.DTOs.Validation.UpdateValidation;

public class AttendeeUpdateRequestDTO_Validation : AbstractValidator<AttendeeUpdateRequestDTO>
{
    public AttendeeUpdateRequestDTO_Validation()
    {
        RuleFor(model => model.Email).NotNull().NotEmpty().WithMessage("Email cannot be null");
    }
}