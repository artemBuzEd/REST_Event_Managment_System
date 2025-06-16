using EMS.BLL.DTOs.Request;
using FluentValidation;

namespace EMS.BLL.DTOs.Validation;

public class VenueCreateRequestDTO_Validation : AbstractValidator<VenueCreateRequestDTO>
{
    public VenueCreateRequestDTO_Validation()
    {
        RuleFor(model => model.Name).NotNull().WithMessage("Name cannot be null");
        RuleFor(model => model.City).NotNull().WithMessage("City cannot be null");
        RuleFor(model => model.Country).NotNull().WithMessage("Country cannot be null");
        RuleFor(model => model.Address).NotNull().WithMessage("Address cannot be null");
    }
}