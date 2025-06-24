using EMS.DAL.EF.Entities;
using EMS.DAL.EF.Entities.HelpModels;
using EMS.DAL.EF.Helpers;
using Microsoft.AspNetCore.Identity;

namespace EMS.DAL.EF.Repositories.Contracts;

public interface IEMSAttendeeRepository : IEMSGenericRepository<Attendee>
{
    new IQueryable<Attendee> GetAllAsync();
    Task<IEnumerable<Attendee?>> GetByFirstNameAsync(string firstName);
    Task<IEnumerable<Attendee?>> GetByLastNameAsync(string lastName);
    Task<Attendee?> GetByEmailAsync(string email);
    Task<Attendee?> GetByPhoneNumberAsync(string phoneNumber);
    Task<PagedList<Attendee>> GetAllPaginatedAsync(AttendeeParameters parameters, ISortHelper<Attendee> sortHelper);
    Task<Attendee?> GetByIdAsync(string id);
    
    Task<(IdentityResult, Attendee)> CreateAsync(Attendee attendee, string password);
    new Task<IdentityResult> UpdateAsync(Attendee attendee);
    new Task<IdentityResult> DeleteAsync(Attendee attendee);
}