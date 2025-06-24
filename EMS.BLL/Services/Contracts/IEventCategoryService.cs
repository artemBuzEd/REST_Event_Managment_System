using EMS.BLL.DTOs.Request;
using EMS.BLL.DTOs.Responce;

namespace EMS.BLL.Services.Contracts;

public interface IEventCategoryService
{
    Task<IEnumerable<EventCategoryFullResponseDTO>> GetAllFullAsync();
    Task<IEnumerable<EventCategoryMiniResponseDTO>> GetAllMiniAsync();
    Task<EventCategoryFullResponseDTO> GetByIdAsync(int id);
    Task<IEnumerable<EventCategoryFullResponseDTO>> GetByNameAsync(string name);
    Task<EventCategoryFullResponseDTO> CreateAsync(EventCategoryCreateRequestDTO dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}