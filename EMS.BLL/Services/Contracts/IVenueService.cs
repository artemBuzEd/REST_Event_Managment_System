using EMS.BLL.DTOs.Request;
using EMS.BLL.DTOs.Responce;
using EMS.DAL.EF.Entities.HelpModels;
using EMS.DAL.EF.Helpers;

namespace EMS.BLL.Services.Contracts;

public interface IVenueService
{
    Task<IEnumerable<VenueFullResponseDTO>> GetAllAsync();
    Task<VenueFullResponseDTO> GetByIdAsync(int id);
    Task<IEnumerable<VenueFullResponseDTO>> GetByCityAsync(string city);
    Task<IEnumerable<VenueFullResponseDTO>> GetAllByCapacityRangeAsync(int startCapacity, int endCapacity);
    Task<VenueFullResponseDTO> CreateAsync(VenueCreateRequestDTO request, CancellationToken cancellationToken = default);
    Task<VenueFullResponseDTO> UpdateAsync(int id,VenueUpdateRequestDTO dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedList<VenueFullResponseDTO>> GetAllPaginatedAsync(VenueParameters parameters);
}