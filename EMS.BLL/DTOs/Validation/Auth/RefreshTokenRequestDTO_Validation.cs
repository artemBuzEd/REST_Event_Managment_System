using EMS.BLL.DTOs.Auth;
using FluentValidation;

namespace EMS.BLL.DTOs.Validation.Auth;

public class RefreshTokenRequestDTO_Validation : AbstractValidator<RefreshTokenRequestDTO>
{
    public RefreshTokenRequestDTO_Validation()
    {
        RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Refresh token is required");
    }
}