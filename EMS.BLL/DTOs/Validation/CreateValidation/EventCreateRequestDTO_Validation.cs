using EMS.BLL.DTOs.Request;
using FluentValidation;

namespace EMS.BLL.DTOs.Validation;

public class EventCreateRequestDTO_Validation : AbstractValidator<EventCreateRequestDTO>
{
    public EventCreateRequestDTO_Validation()
    {
        RuleFor(model => model.Name).NotNull().NotEmpty().WithMessage("Name cannot be null");
        RuleFor(model => model.StartTime).NotNull().NotEmpty().WithMessage("Start time cannot be null");
        RuleFor(model => model.EndTime).NotNull().NotEmpty().WithMessage("End time cannot be null");
    }
}