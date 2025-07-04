using EMS.BLL.DTOs.Request;
using EMS.BLL.DTOs.Responce;
using EMS.DAL.EF.Entities.HelpModels;
using EMS.DAL.EF.Helpers;

namespace EMS.BLL.Services.Contracts;

public interface IOrganizerService
{
    Task<IEnumerable<OrganizerFullResponseDTO>> GetAllAsync();
    Task<OrganizerFullResponseDTO> GetByIdAsync(string id);
    Task<OrganizerFullResponseDTO> CreateAsync(OrganizerCreateRequestDTO dto, CancellationToken cancellationToken = default);
    Task<OrganizerFullResponseDTO> UpdateAsync(string id, OrganizerUpdateRequestDTO dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<PagedList<OrganizerFullResponseDTO>> GetAllPaginatedAsync(OrganizerParameters parameters);
}