using EMS.BLL.DTOs.Request;
using EMS.BLL.DTOs.Responce;
using FluentValidation;

namespace EMS.BLL.DTOs.Validation;

public class EventCategoryCreateDTO_Validation : AbstractValidator<EventCategoryCreateRequestDTO>
{
    public EventCategoryCreateDTO_Validation()
    {
        RuleFor(model => model.Name).NotNull().NotEmpty().WithMessage("Name cannot be null");
    }
}