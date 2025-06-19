using EMS.BLL.DTOs.Request;
using EMS.BLL.DTOs.Responce;
using EMS.BLL.Exceptions;
using EMS.BLL.Services.Contracts;
using EMS.DAL.EF.Entities;
using EMS.DAL.EF.UOW.Contract;
using Mapster;

namespace EMS.BLL.Services;

public class EventCategoryService : IEventCategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public EventCategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<EventCategoryFullResponseDTO>> GetAllFullAsync()
    {
        var eventCategories = await _unitOfWork.EventCategories.GetAllAsync();
        return eventCategories.Adapt<IEnumerable<EventCategoryFullResponseDTO>>();
    }

    public async Task<IEnumerable<EventCategoryMiniResponseDTO>> GetAllMiniAsync()
    {
        var eventCategories = await _unitOfWork.EventCategories.GetAllAsync();
        return eventCategories.Adapt<IEnumerable<EventCategoryMiniResponseDTO>>();
    }

    public async Task<EventCategoryFullResponseDTO> GetByIdAsync(int id)
    {
        var eventCategory = await isExistsAsync(id);
        return eventCategory.Adapt<EventCategoryFullResponseDTO>();
    }

    public async Task<EventCategoryFullResponseDTO> GetByNameAsync(string name)
    {
        var eventCategory = await _unitOfWork.EventCategories.GetByNameAsync(name);
        if (eventCategory == null)
        {
            throw new ApplicationException($"Event category with name {name} does not exist. Or some error occured.");
        }
        return eventCategory.Adapt<EventCategoryFullResponseDTO>();
    }

    public async Task<EventCategoryFullResponseDTO> CreateAsync(EventCategoryCreateRequestDTO dto, CancellationToken cancellationToken = default)
    {
        var eventCategoryToCreate = dto.Adapt<EventCategory>();
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.EventCategories.AddAsync(eventCategoryToCreate);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return eventCategoryToCreate.Adapt<EventCategoryFullResponseDTO>();
        }
        catch(Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new ApplicationException($"There was an error creating the event category [Task<EventCategoryFullResponseDTO> CreateAsync()] {e}");
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var eventCategoryToDelete = await isExistsAsync(id);
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.EventCategories.DeleteAsync(eventCategoryToDelete);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new ApplicationException("There was an error deleting the eventCategory", e);
        }
    }
    private async Task<EventCategory> isExistsAsync(int id)
    {
        var eventCategory = await _unitOfWork.EventCategories.GetByIdAsync(id);
        if (eventCategory == null)
        {
            throw new NotFoundException($"Attendee with id {id} does not exist");
        }

        return eventCategory;
    }
}