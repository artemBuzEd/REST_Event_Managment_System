using EMS.BLL.DTOs.Request;
using EMS.BLL.DTOs.Request.Attendee;
using EMS.BLL.DTOs.Responce;
using EMS.DAL.EF.Entities.HelpModels;
using EMS.DAL.EF.Helpers;
using Microsoft.AspNetCore.Identity;

namespace EMS.BLL.Services.Contracts;

public interface IAttendeeService
{
    Task<IEnumerable<AttendeeFullResponseDTO>> GetAllAsync();
    Task<AttendeeFullResponseDTO> GetByIdAsync(string id);
    Task<AttendeeFullResponseDTO> CreateAsync(AttendeeCreateRequestDTO dto, CancellationToken cancellationToken = default);
    Task<AttendeeFullResponseDTO> UpdateAsync(string id,AttendeeUpdateRequestDTO dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<PagedList<AttendeeFullResponseDTO>> GetAllPaginatedAsync(AttendeeParameters parameters);
}