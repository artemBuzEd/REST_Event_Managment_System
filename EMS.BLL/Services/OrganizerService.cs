using System.ComponentModel.DataAnnotations;
using EMS.BLL.DTOs.Request;
using EMS.BLL.DTOs.Responce;
using EMS.BLL.Exceptions;
using EMS.BLL.Services.Contracts;
using EMS.DAL.EF.Entities;
using EMS.DAL.EF.Entities.HelpModels;
using EMS.DAL.EF.Helpers;
using EMS.DAL.EF.UOW.Contract;
using Mapster;

namespace EMS.BLL.Services;

public class OrganizerService : IOrganizerService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrganizerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<OrganizerFullResponseDTO>> GetAllAsync()
    {
        var organizers = await _unitOfWork.Organizers.GetAllAsync();
        return organizers.Adapt<IEnumerable<OrganizerFullResponseDTO>>();
    }

    public async Task<OrganizerFullResponseDTO> GetByIdAsync(int id)
    {
        var organizer = await isExists(id);
        return organizer.Adapt<OrganizerFullResponseDTO>();
    }
    
    public async Task<OrganizerFullResponseDTO> CreateAsync(OrganizerCreateRequestDTO dto, CancellationToken cancellationToken = default)
    {
        var isExists = await _unitOfWork.Organizers.GetByEmailAsync(dto.Email);
        if (isExists != null)
        {
            throw new ValidationException("Organizer with same email is already exists");
        }
        var organizerToCreate = dto.Adapt<Organizer>();
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.Organizers.AddAsync(organizerToCreate);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return organizerToCreate.Adapt<OrganizerFullResponseDTO>();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new ApplicationException("There was an error creating the organizer [Task<OrganizerFullResponseDTO> CreateAsync()]", e);
        }
    }

    public async Task<OrganizerFullResponseDTO> UpdateAsync(int id, OrganizerUpdateRequestDTO dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var organizerToChange = await isExists(id);
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            dto.Adapt(organizerToChange);
            await _unitOfWork.Organizers.UpdateAsync(organizerToChange);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return organizerToChange.Adapt<OrganizerFullResponseDTO>();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new ApplicationException("There was an error updating the organizer", e);
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var organizerToDelete = await isExists(id);
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.Organizers.DeleteAsync(organizerToDelete);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new ApplicationException("There was an error deleting the organizer", e);
        }
    }
    
    public async Task<PagedList<OrganizerFullResponseDTO>> GetAllPaginatedAsync(OrganizerParameters parameters)
    {
        var pagedOrganizer = await _unitOfWork.Organizers.GetAllPaginatedAsync(parameters, new SortHelper<Organizer>());
        
        var mapped = pagedOrganizer.Select(a => a.Adapt<OrganizerFullResponseDTO>()).ToList();
        
        return new PagedList<OrganizerFullResponseDTO>(mapped, pagedOrganizer.TotalCount, pagedOrganizer.CurrentPage, pagedOrganizer.PageSize);
    }
    private async Task<Organizer> isExists(int id)
    {
        var organizer = await _unitOfWork.Organizers.GetByIdAsync(id);
        if (organizer == null)
        {
            throw new NotFoundException($"Organizer with id {id} does not exist");
        }
        return organizer;
    }
}