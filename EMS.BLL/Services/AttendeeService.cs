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
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EMS.BLL.Services;

public class AttendeeService : IAttendeeService
{
    private readonly IUnitOfWork _unitOfWork;

    public AttendeeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<AttendeeFullResponseDTO>> GetAllAsync()
    {
        var attendees = await _unitOfWork.Attendees.GetAllAsync();
        return attendees.Adapt<IEnumerable<AttendeeFullResponseDTO>>();
    }

    public async Task<AttendeeFullResponseDTO> GetByIdAsync(int id)
    {
        var attendee = await isExistsAsync(id);
        return attendee.Adapt<AttendeeFullResponseDTO>();
    }

    public async Task<AttendeeFullResponseDTO> CreateAsync(AttendeeCreateRequestDTO dto, CancellationToken cancellationToken)
    {
        var isExists = await _unitOfWork.Attendees.GetByEmailAsync(dto.Email);
        if (isExists != null)
        {
            throw new ValidationException("Attendee with same email is already exists");
        }
        var attendeeToCreate = dto.Adapt<Attendee>();
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            _unitOfWork.Attendees.AddAsync(attendeeToCreate);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return attendeeToCreate.Adapt<AttendeeFullResponseDTO>();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new ApplicationException("There was an error creating the attendee [Task<AttendeeFullResponseDTO> CreateAsync()]", e);
        }
    }

    public async Task<AttendeeFullResponseDTO> UpdateAsync(int id, AttendeeUpdateRequestDTO dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var attendeeToChange = await isExistsAsync(id);
            dto.Adapt(attendeeToChange);
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.Attendees.UpdateAsync(attendeeToChange);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return attendeeToChange.Adapt<AttendeeFullResponseDTO>();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new ApplicationException("There was an error updating the attendee [Task<AttendeeFullResponseDTO> UpdateAsync()]", e);
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var attendeeToDelete = await isExistsAsync(id);
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.Attendees.DeleteAsync(attendeeToDelete);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new ApplicationException("There was an error deleting the attendee", e);
        }
        
    }

    public async Task<PagedList<AttendeeFullResponseDTO>> GetAllPaginatedAsync(AttendeeParameters parameters)
    {
        var pagedAttendees = await _unitOfWork.Attendees.GetAllPaginatedAsync(parameters, new SortHelper<Attendee>());
        
        var mapped = pagedAttendees.Select(a => a.Adapt<AttendeeFullResponseDTO>()).ToList();
        
        return new PagedList<AttendeeFullResponseDTO>(mapped, pagedAttendees.TotalCount, pagedAttendees.CurrentPage, pagedAttendees.PageSize);
    }
    private async Task<Attendee> isExistsAsync(int id)
    {
        var attendee = await _unitOfWork.Attendees.GetByIdAsync(id);
        if (attendee == null)
        {
            throw new NotFoundException($"Attendee with id {id} does not exist");
        }
        return attendee;
    }
}