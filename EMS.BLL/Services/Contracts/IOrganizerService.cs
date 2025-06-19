using EMS.BLL.DTOs.Request;
using EMS.BLL.DTOs.Responce;

namespace EMS.BLL.Services.Contracts;

public interface IOrganizerService
{
    Task<IEnumerable<OrganizerFullResponseDTO>> GetAllAsync();
    Task<OrganizerFullResponseDTO> GetByIdAsync(int id);
    Task<OrganizerFullResponseDTO> CreateAsync(OrganizerCreateRequestDTO dto, CancellationToken cancellationToken = default);
    Task<OrganizerFullResponseDTO> UpdateAsync(int id, OrganizerUpdateRequestDTO dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}