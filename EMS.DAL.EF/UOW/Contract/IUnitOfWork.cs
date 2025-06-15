using EMS.DAL.EF.Repositories.Contracts;

namespace EMS.DAL.EF.UOW.Contract;

public interface IUnitOfWork
{
    IEMSAttendeeRepository Attendees { get; }
    IEMSEventRepository Events { get; }
    IEMSEventCategoryRepository EventCategories { get; }
    IEMSOrganizerRepository Organizers { get; }
    IEMSVenueRepository Venues { get; }
    IEMSRegistrationRepository Registrations { get; }
    
    Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitTransactionAsync(CancellationToken cancellationToken);
    Task RollbackTransactionAsync(CancellationToken cancellationToken);
    Task<int> CompleteAsync(CancellationToken cancellationToken);
    void Dispose();
}