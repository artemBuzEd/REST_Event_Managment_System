using EMS.BLL.DTOs.Request;
using FluentValidation;

namespace EMS.BLL.DTOs.Validation.UpdateValidation;

public class VenueUpdateRequestDTO_Validation : AbstractValidator<VenueUpdateRequestDTO>
{
    public VenueUpdateRequestDTO_Validation()
    {
        RuleFor(model => model.Name).NotNull().NotEmpty().WithMessage("Name is required.");
        RuleFor(model => model.Address).NotNull().NotEmpty().WithMessage("Address is required.");
        RuleFor(model => model.City).NotNull().NotEmpty().WithMessage("City is required.");
        RuleFor(model => model.Country).NotNull().NotEmpty().WithMessage("Country is required.");
    }
}