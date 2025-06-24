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
using Microsoft.EntityFrameworkCore;

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
        var organizers = await _unitOfWork.Organizers.GetAllAsync().ToListAsync();
        return organizers.Adapt<IEnumerable<OrganizerFullResponseDTO>>();
    }

    public async Task<OrganizerFullResponseDTO> GetByIdAsync(string id)
    {
        var organizer = await isExists(id);
        return organizer.Adapt<OrganizerFullResponseDTO>();
    }
    
    public async Task<OrganizerFullResponseDTO> CreateAsync(OrganizerCreateRequestDTO dto, CancellationToken cancellationToken = default)
    {
        if((await _unitOfWork.Organizers.GetByEmailAsync(dto.Email)).Any())
            throw new ValidationException("Organizer with the same e-mail already exists");
        
        var organizerToCreate = dto.Adapt<Organizer>();
        organizerToCreate.UserName = dto.Email;
        
        var(result, created) = await _unitOfWork.Organizers.CreateAsync(organizerToCreate, dto.Password);
        
        if(!result.Succeeded)
            throw new ValidationException(string.Join("; ",
                result.Errors.Select(e => e.Description)));
        
        return created.Adapt<OrganizerFullResponseDTO>();
    }

    public async Task<OrganizerFullResponseDTO> UpdateAsync(string id, OrganizerUpdateRequestDTO dto, CancellationToken cancellationToken = default)
    {
        var organizer = await isExists(id);
        dto.Adapt(organizer);
        
        var result = await _unitOfWork.Organizers.UpdateAsync(organizer);
        if (!result.Succeeded)
            throw new ValidationException(string.Join("; ",
                result.Errors.Select(e => e.Description)));
        
        return dto.Adapt<OrganizerFullResponseDTO>();
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var organizer = await isExists(id);
        
        var result = await _unitOfWork.Organizers.DeleteAsync(organizer);
        if (!result.Succeeded)
            throw new ValidationException(string.Join("; ",
                result.Errors.Select(e => e.Description)));
    }
    
    public async Task<PagedList<OrganizerFullResponseDTO>> GetAllPaginatedAsync(OrganizerParameters parameters)
    {
        var pagedOrganizer = await _unitOfWork.Organizers.GetAllPaginatedAsync(parameters, new SortHelper<Organizer>());
        
        var mapped = pagedOrganizer.Select(a => a.Adapt<OrganizerFullResponseDTO>()).ToList();
        
        return new PagedList<OrganizerFullResponseDTO>(mapped, pagedOrganizer.TotalCount, pagedOrganizer.CurrentPage, pagedOrganizer.PageSize);
    }
    private async Task<Organizer> isExists(string id)
    {
        var organizer = await _unitOfWork.Organizers.GetByIdAsync(id);
        if (organizer == null)
        {
            throw new NotFoundException($"Organizer with id {id} does not exist");
        }
        return organizer;
    }
}