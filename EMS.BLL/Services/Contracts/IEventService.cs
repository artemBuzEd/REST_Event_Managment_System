using EMS.BLL.DTOs.Request;
using EMS.BLL.DTOs.Responce;
using EMS.BLL.DTOs.Validation;

namespace EMS.BLL.Services.Contracts;

public interface IEventService
{
    Task<IEnumerable<EventMiniResponse>> GetAllAsync();
    Task<EventMiniResponse> GetByIdAsync(int id);
    Task<IEnumerable<EventMiniResponse>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<EventFullResponseDTO> GetDetailedEventByIdAsync(int id);
    Task<EventCreateRequestDTO> CreateAsync(EventCreateRequestDTO dto, CancellationToken cancellationToken = default);
    Task<EventUpdateRequestDTO> UpdateAsync(int id, EventUpdateRequestDTO dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}