using EMS.BLL.DTOs.Request.Registration;
using EMS.BLL.DTOs.Responce;
using EMS.BLL.Services.Contracts;
using EMS.DAL.EF.Entities;
using EMS.DAL.EF.UOW.Contract;
using FluentValidation;
using Mapster;

namespace EMS.BLL.Services;

public class RegistrationService : IRegistrationService
{
    private readonly IUnitOfWork _unitOfWork;

    public RegistrationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<RegistrationFullResponseDTO> GetByIdAsync(int id)
    {
        var registration = await isExistsAsync(id);
        return registration.Adapt<RegistrationFullResponseDTO>();
    }

    public async Task<IEnumerable<RegistrationFullResponseDTO>> GetByRegistrationDateAsync(DateTime registrationDate)
    {
        var registrations = await _unitOfWork.Registrations.GetByRegistrationDateAsync(registrationDate);
        return registrations.Adapt<IEnumerable<RegistrationFullResponseDTO>>();
    }

    public async Task<IEnumerable<RegistrationFullResponseDTO>> GetAllRegistrationsOnSpecificEventIdAsync(int id)
    {
        var registrations = await _unitOfWork.Registrations.GetAllBySpecificEventAsync(id);
        return registrations.Adapt<IEnumerable<RegistrationFullResponseDTO>>();
    }

    public async Task<RegistrationFullResponseDTO> CreateAsync(RegistrationCreateRequestDTO dto, CancellationToken cancellationToken = default)
    {
        var isExist = await _unitOfWork.Registrations.GetAllByAttendeeAndEventId(dto.AttendeeId, dto.EventId);
        if (isExist != null)
        {
            throw new ValidationException($"Registration already exists: {dto.AttendeeId}/{dto.EventId}");
        }
        var registrationToCreate = dto.Adapt<Registration>();
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.Registrations.AddAsync(registrationToCreate);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return registrationToCreate.Adapt<RegistrationFullResponseDTO>();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new ApplicationException($"Error creating registration: [{dto.AttendeeId}/{dto.EventId}] ", e);
        }
    }
    
    public async Task<RegistrationFullResponseDTO> UpdateAsync(int id,RegistrationUpdateRequestDTO dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var registrationToChange = await isExistsAsync(id);
            dto.Adapt(registrationToChange);
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.Registrations.UpdateAsync(registrationToChange);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return registrationToChange.Adapt<RegistrationFullResponseDTO>();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new ApplicationException($"Error updating registration: [{id}] ", e);
        }
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var registrationToDelete = await isExistsAsync(id);
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.Registrations.DeleteAsync(registrationToDelete);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new ApplicationException($"Error deleting registration: [{id}] ", e);
        }
    }
    private async Task<Registration> isExistsAsync(int id)
    {
        var registration = await _unitOfWork.Registrations.GetByIdAsync(id);
        if (registration == null)
        {
            throw new ApplicationException($"Registration with id: {id} not found");
        }
        return registration;
    }
}