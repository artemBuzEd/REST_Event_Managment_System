using EMS.DAL.EF.Data;
using EMS.DAL.EF.Repositories.Contracts;
using EMS.DAL.EF.UOW.Contract;
using Microsoft.EntityFrameworkCore.Storage;

namespace EMS.DAL.EF.UOW;

public class UnitOfWork : IUnitOfWork
{
    private readonly EMSManagmentDbContext _context;
    private IDbContextTransaction _transaction;
    
    public IEMSAttendeeRepository Attendees { get; }
    public IEMSEventRepository Events { get; }
    public IEMSEventCategoryRepository EventCategories { get; }
    public IEMSOrganizerRepository Organizers { get; }
    public IEMSRegistrationRepository Registrations { get; }
    public IEMSVenueRepository Venues { get; }

    public UnitOfWork(EMSManagmentDbContext context, 
        IEMSAttendeeRepository attendees, 
        IEMSEventRepository events, 
        IEMSEventCategoryRepository eventCategories, 
        IEMSOrganizerRepository organizers, 
        IEMSRegistrationRepository registrations, 
        IEMSVenueRepository venues)
    {
        _context = context;
        Attendees = attendees;
        Events = events;
        EventCategories = eventCategories;
        Organizers = organizers;
        Registrations = registrations;
        Venues = venues;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default) 
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("The transaction has not been started. [CommitTransactionAsync()]");
        }
        await _transaction.CommitAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("The transaction has not been started. [RollbackTransactionAsync()]");
        }
        await _transaction.RollbackAsync(cancellationToken);
    }

    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        
        _context?.Dispose();
    }
}