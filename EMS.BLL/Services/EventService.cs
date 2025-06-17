using EMS.BLL.DTOs.Request;
using EMS.BLL.DTOs.Responce;
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

    public async Task<IEnumerable<EventMiniResponse>> GetAllAsync()
    {
        var events = await _unitOfWork.Events.GetAllAsync();
        return events.Adapt<IEnumerable<EventMiniResponse>>();
    }

    public async Task<EventMiniResponse> GetByIdAsync(int id)
    {
        var _event = await isExists(id);
        return _event.Adapt<EventMiniResponse>();
    }

    public async Task<IEnumerable<EventMiniResponse>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var events = await _unitOfWork.Events.GetByDateRangeAsync(startDate, endDate);
        return events.Adapt<IEnumerable<EventMiniResponse>>();
    }

    public async Task<EventFullResponseDTO> GetDetailedEventByIdAsync(int id)
    {
        var _event = await isExists(id);
        return _event.Adapt<EventFullResponseDTO>();
    }

    public async Task<EventCreateRequestDTO> CreateAsync(EventCreateRequestDTO dto, CancellationToken cancellationToken = default)
    {
        var eventToCreate = dto.Adapt<Event>();
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.Events.AddAsync(eventToCreate);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return dto.Adapt<EventCreateRequestDTO>();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new ApplicationException("Error creating event", e);
        }
    }

    public async Task<EventUpdateRequestDTO> UpdateAsync(int id, EventUpdateRequestDTO dto,
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
            return dto.Adapt<EventUpdateRequestDTO>();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new ApplicationException("Error updating event", e);
        }
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var eventToDelete = await isExists(id);
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.Events.DeleteAsync(eventToDelete);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return true;
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
            throw new ApplicationException($"Event with id {id} not found");
        }

        return _event;
    }
}