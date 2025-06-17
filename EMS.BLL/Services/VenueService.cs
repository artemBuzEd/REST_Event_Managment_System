using EMS.BLL.DTOs.Request;
using EMS.BLL.DTOs.Responce;
using EMS.BLL.Services.Contracts;
using EMS.DAL.EF.Entities;
using EMS.DAL.EF.UOW.Contract;
using Mapster;

namespace EMS.BLL.Services;

public class VenueService : IVenueService
{
    private readonly IUnitOfWork _unitOfWork;

    public VenueService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<VenueFullResponseDTO>> GetAllAsync()
    {
        var venues = await _unitOfWork.Venues.GetAllAsync();
        return venues.Adapt<IEnumerable<VenueFullResponseDTO>>();
    }

    public async Task<VenueFullResponseDTO> GetByIdAsync(int id)
    {
        var venue = await isExistsAsync(id);
        return venue.Adapt<VenueFullResponseDTO>();
    }

    public async Task<IEnumerable<VenueFullResponseDTO>> GetByCityAsync(string city)
    {
        var venues = await _unitOfWork.Venues.GetByCityAsync(city);
        return venues.Adapt<IEnumerable<VenueFullResponseDTO>>();
    }

    public async Task<IEnumerable<VenueFullResponseDTO>> GetAllByCapacityRangeAsync(int startCapacity, int endCapacity)
    {
        var venues = await _unitOfWork.Venues.GetByCapacityRangeAsync(startCapacity, endCapacity);
        return venues.Adapt<IEnumerable<VenueFullResponseDTO>>();
    }

    public async Task<VenueFullResponseDTO> CreateAsync(VenueCreateRequestDTO dto, CancellationToken cancellationToken = default)
    {
        var isExist = await _unitOfWork.Venues.GetByAddressAsync(dto.Address);
        if (isExist != null)
        {
            throw new ApplicationException("Venue with same address already exists");
        }
        var venueToCreate = dto.Adapt<Venue>();
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.Venues.AddAsync(venueToCreate);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return venueToCreate.Adapt<VenueFullResponseDTO>();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new ApplicationException("Error creating venue", e);
        }
    }

    public async Task<VenueFullResponseDTO> UpdateAsync(int id, VenueUpdateRequestDTO dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var venueToChange = await isExistsAsync(id);
            dto.Adapt(venueToChange);
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.Venues.UpdateAsync(venueToChange);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return venueToChange.Adapt<VenueFullResponseDTO>();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new ApplicationException("Error updating venue", e);
        }
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var venue = await isExistsAsync(id);
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.Venues.DeleteAsync(venue);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new ApplicationException("Error deleting venue", e);
        }
    }
    private async Task<Venue> isExistsAsync(int id)
    {
        var venue = await _unitOfWork.Venues.GetByIdAsync(id);
        if (venue == null)
        {
            throw new ApplicationException($"Venue with id {id} does not exist.");
        }
        return venue;
    } 
}