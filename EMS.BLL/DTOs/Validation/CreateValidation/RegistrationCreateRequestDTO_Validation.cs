using EMS.BLL.DTOs.Request.Registration;
using FluentValidation;

namespace EMS.BLL.DTOs.Validation;

public class RegistrationCreateRequestDTO_Validation : AbstractValidator<RegistrationCreateRequestDTO>
{
    public RegistrationCreateRequestDTO_Validation()
    {
        RuleFor(model => model.AttendeeId).NotNull().NotEmpty().WithMessage("AttendeeId is required.");
        RuleFor(model => model.EventId).NotNull().NotEmpty().WithMessage("EventId is required.");
    }
}