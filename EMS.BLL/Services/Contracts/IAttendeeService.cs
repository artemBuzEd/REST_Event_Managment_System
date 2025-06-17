using EMS.BLL.DTOs.Request;
using EMS.BLL.DTOs.Request.Attendee;
using EMS.BLL.DTOs.Responce;

namespace EMS.BLL.Services.Contracts;

public interface IAttendeeService
{
    Task<IEnumerable<AttendeeFullResponseDTO>> GetAllAsync();
    Task<AttendeeFullResponseDTO> GetByIdAsync(int id);
    Task<AttendeeFullResponseDTO> CreateAsync(AttendeeCreateRequestDTO dto, CancellationToken cancellationToken);
    Task<AttendeeFullResponseDTO> UpdateAsync(int id,AttendeeUpdateRequestDTO dto, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}