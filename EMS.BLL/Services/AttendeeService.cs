using System.ComponentModel.DataAnnotations;
using EMS.BLL.DTOs.Request;
using EMS.BLL.DTOs.Request.Attendee;
using EMS.BLL.DTOs.Responce;
using EMS.BLL.Exceptions;
using EMS.BLL.Services.Contracts;
using EMS.DAL.EF.Entities;
using EMS.DAL.EF.Entities.HelpModels;
using EMS.DAL.EF.Helpers;
using EMS.DAL.EF.UOW.Contract;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EMS.BLL.Services;

public class AttendeeService : IAttendeeService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public AttendeeService(IUnitOfWork unitOfWork, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<AttendeeFullResponseDTO>> GetAllAsync()
    {
        var attendees = await _unitOfWork.Attendees.GetAllAsync().ToListAsync();
        return attendees.Adapt<IEnumerable<AttendeeFullResponseDTO>>();
    }

    public async Task<AttendeeFullResponseDTO> GetByIdAsync(string id)
    {
        var attendee = await isExistsAsync(id);
        return attendee.Adapt<AttendeeFullResponseDTO>();
    }

    public async Task<AttendeeFullResponseDTO> CreateAsync(AttendeeCreateRequestDTO dto, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Attendees.GetByEmailAsync(dto.Email) != null)
            throw new ValidationException("An attendee with the same e-mail already exists");
        
        
        var attendeeToCreate = dto.Adapt<Attendee>();
        attendeeToCreate.UserName = dto.Email;
        
        var (result, createdAttendee) = await _unitOfWork.Attendees.CreateAsync(attendeeToCreate, dto.Password);
        if(!result.Succeeded)
            throw new ValidationException(string.Join("; ", result.Errors.Select(e => e.Description)));
        
        return attendeeToCreate.Adapt<AttendeeFullResponseDTO>();
    }

    public async Task<AttendeeFullResponseDTO> UpdateAsync(string id, AttendeeUpdateRequestDTO dto, CancellationToken cancellationToken = default)
    {
            var attendeeToChange = await isExistsAsync(id);
            dto.Adapt(attendeeToChange);
            
            var result = await _unitOfWork.Attendees.UpdateAsync(attendeeToChange);
            if(!result.Succeeded)
                throw new ValidationException(string.Join("; ", result.Errors.Select(e => e.Description)));
            
            return attendeeToChange.Adapt<AttendeeFullResponseDTO>();
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var attendeeToDelete = await isExistsAsync(id);
        var result = await _unitOfWork.Attendees.DeleteAsync(attendeeToDelete);
        
        if(!result.Succeeded)
            throw new ValidationException(string.Join("; ", result.Errors.Select(e => e.Description)));
    }

    public async Task<PagedList<AttendeeFullResponseDTO>> GetAllPaginatedAsync(AttendeeParameters parameters)
    {
        var pagedAttendees = await _unitOfWork.Attendees.GetAllPaginatedAsync(parameters, new SortHelper<Attendee>());
        
        var mapped = pagedAttendees.Select(a => a.Adapt<AttendeeFullResponseDTO>()).ToList();
        
        return new PagedList<AttendeeFullResponseDTO>(mapped, pagedAttendees.TotalCount, pagedAttendees.CurrentPage, pagedAttendees.PageSize);
    }
    private async Task<Attendee> isExistsAsync(string id)
    {
        var attendee = await _unitOfWork.Attendees.GetByIdAsync(id);
        if (attendee == null)
        {
            throw new NotFoundException($"Attendee with id {id} does not exist");
        }
        return attendee;
    }
}