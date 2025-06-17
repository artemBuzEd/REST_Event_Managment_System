using EMS.BLL.DTOs.Request;
using EMS.BLL.DTOs.Responce;

namespace EMS.BLL.Services.Contracts;

public interface IOrganizerService
{
    Task<OrganizerFullResponseDTO> CreateAsync();
    Task<OrganizerFullResponseDTO> UpdateAsync(int id, OrganizerUpdateRequestDTO dto);
    Task<bool> DeleteAsync(int id);
}