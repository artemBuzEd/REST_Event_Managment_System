using EMS.BLL.DTOs.Request;
using EMS.BLL.DTOs.Responce;

namespace EMS.BLL.Services.Contracts;

public interface IVenueService
{
    Task<IEnumerable<VenueFullResponseDTO>> GetAllAsync();
    Task<VenueFullResponseDTO> GetByIdAsync(int id);
    Task<IEnumerable<VenueFullResponseDTO>> GetByCityAsync(string city);
    Task<IEnumerable<VenueFullResponseDTO>> GetAllByCapacityRangeAsync(int startCapacity, int endCapacity);
    Task<VenueFullResponseDTO> CreateAsync();
    Task<VenueFullResponseDTO> UpdateAsync(int id,VenueUpdateRequestDTO dto);
    Task<bool> DeleteAsync(int id);
}