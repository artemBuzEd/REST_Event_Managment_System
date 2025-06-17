using EMS.BLL.DTOs.Request;
using FluentValidation;

namespace EMS.BLL.DTOs.Validation.UpdateValidation;

public class EventUpdateRequestDTO_Validation : AbstractValidator<EventUpdateRequestDTO>
{
    public EventUpdateRequestDTO_Validation()
    {
        RuleFor(model => model.Name).NotNull().WithMessage("Name cannot be null");
        RuleFor(model => model.StartTime).NotNull().WithMessage("Start time cannot be null");
        RuleFor(model => model.EndTime).NotNull().WithMessage("End time cannot be null");
        RuleFor(model => model.VenueId).NotNull().WithMessage("Venue id cannot be null");
        RuleFor(model => model.EventCategoryId).NotNull().WithMessage("Event category id cannot be null");
    }
}