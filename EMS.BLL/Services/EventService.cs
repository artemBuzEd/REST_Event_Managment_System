using EMS.BLL.DTOs.Request;
using EMS.BLL.DTOs.Responce;
using EMS.BLL.Exceptions;
using EMS.BLL.Services.Contracts;
using EMS.DAL.EF.Entities;
using EMS.DAL.EF.UOW.Contract;
using Mapster;

namespace EMS.BLL.Services;

public class EventService : IEventService
{
    private readonly IUnitOfWork _unitOfWork;

    public EventService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<EventMiniResponseDTO>> GetAllAsync()
    {
        var events = await _unitOfWork.Events.GetAllAsync();
        return events.Adapt<IEnumerable<EventMiniResponseDTO>>();
    }

    public async Task<EventMiniResponseDTO> GetByIdAsync(int id)
    {
        var _event = await isExists(id);
        return _event.Adapt<EventMiniResponseDTO>();
    }

    public async Task<IEnumerable<EventMiniResponseDTO>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var events = await _unitOfWork.Events.GetByDateRangeAsync(startDate, endDate);
        return events.Adapt<IEnumerable<EventMiniResponseDTO>>();
    }

    public async Task<EventFullResponseDTO> GetDetailedEventByIdAsync(int id)
    {
        var _event = await _unitOfWork.Events.GetByIdWithVenueAndEventCategoryAndOrganizerAsync(id);
        if (_event == null)
        {
            throw new NotFoundException($"Event not found with this id: {id}");
        }
        return _event.Adapt<EventFullResponseDTO>();
    }

    public async Task<IEnumerable<EventFullResponseDTO>> GetDetailedEventSpecificOrganizerIdAsync(int organizerId)
    {
        var events = await _unitOfWork.Events.GetAllByOrganizerIdAsync(organizerId);
        return events.Adapt<IEnumerable<EventFullResponseDTO>>();
    }

    public async Task<EventMiniResponseDTO> CreateAsync(EventCreateRequestDTO dto, CancellationToken cancellationToken = default)
    {
        var eventToCreate = dto.Adapt<Event>();
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.Events.AddAsync(eventToCreate);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return dto.Adapt<EventMiniResponseDTO>();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new ApplicationException("Error creating event", e);
        }
    }

    public async Task<EventMiniResponseDTO> UpdateAsync(int id, EventUpdateRequestDTO dto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var eventToChange = await isExists(id);
            dto.Adapt(eventToChange);
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.Events.UpdateAsync(eventToChange);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return dto.Adapt<EventMiniResponseDTO>();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new ApplicationException("Error updating event", e);
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var eventToDelete = await isExists(id);
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.Events.DeleteAsync(eventToDelete);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new ApplicationException("Error deleting event", e);
        }
    }
    private async Task<Event> isExists(int id)
    {
        var _event = await _unitOfWork.Events.GetByIdAsync(id);
        if (_event == null)
        {
            throw new NotFoundException($"Event with id {id} not found");
        }

        return _event;
    }
}