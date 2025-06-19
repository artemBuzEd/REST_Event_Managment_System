using EMS.BLL.DTOs.Request.Registration;
using EMS.BLL.DTOs.Responce;

namespace EMS.BLL.Services.Contracts;

public interface IRegistrationService
{
    Task<RegistrationFullResponseDTO> GetByIdAsync(int id);
    Task<IEnumerable<RegistrationFullResponseDTO>> GetByRegistrationDateAsync(DateTime registrationDate);
    Task<IEnumerable<RegistrationFullResponseDTO>> GetAllRegistrationsOnSpecificEventIdAsync(int id);
    Task<RegistrationFullResponseDTO> CreateAsync(RegistrationCreateRequestDTO dto, CancellationToken cancellationToken = default);
    Task<RegistrationFullResponseDTO> UpdateAsync(int id, RegistrationUpdateRequestDTO dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}