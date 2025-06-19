using EMS.BLL.DTOs.Request;
using EMS.BLL.DTOs.Responce;
using EMS.BLL.DTOs.Validation;

namespace EMS.BLL.Services.Contracts;

public interface IEventService
{
    Task<IEnumerable<EventMiniResponseDTO>> GetAllAsync();
    Task<EventMiniResponseDTO> GetByIdAsync(int id);
    Task<IEnumerable<EventMiniResponseDTO>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<EventFullResponseDTO> GetDetailedEventByIdAsync(int id);
    Task<IEnumerable<EventFullResponseDTO>> GetDetailedEventSpecificOrganizerIdAsync(int organizerId);
    Task<EventMiniResponseDTO> CreateAsync(EventCreateRequestDTO dto, CancellationToken cancellationToken = default);
    Task<EventMiniResponseDTO> UpdateAsync(int id, EventUpdateRequestDTO dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}