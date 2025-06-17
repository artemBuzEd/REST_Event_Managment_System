using EMS.BLL.DTOs.Request;
using EMS.BLL.DTOs.Responce;

namespace EMS.BLL.Services.Contracts;

public interface IEventCategoryService
{
    Task<IEnumerable<EventCategoryFullResponseDTO>> GetAllFullAsync();
    Task<IEnumerable<EventCategoryMiniResponseDTO>> GetAllMiniAsync();
    Task<EventCategoryFullResponseDTO> GetByIdAsync();
    Task<EventCategoryFullResponseDTO> CreateAsync(int id,EventCategoryCreateRequestDTO dto);
    Task<bool> DeleteAsync(int id);
}