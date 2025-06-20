using EMS.DAL.EF.Entities;
using EMS.DAL.EF.Entities.HelpModels;
using EMS.DAL.EF.Helpers;

namespace EMS.DAL.EF.Repositories.Contracts;

public interface IEMSAttendeeRepository : IEMSGenericRepository<Attendee>
{
    Task<IEnumerable<Attendee?>> GetByFirstNameAsync(string firstName);
    Task<IEnumerable<Attendee?>> GetByLastNameAsync(string lastName);
    Task<Attendee?> GetByEmailAsync(string email);
    Task<Attendee?> GetByPhoneNumberAsync(string phoneNumber);
    Task<PagedList<Attendee>> GetAllPaginatedAsync(AttendeeParameters parameters, ISortHelper<Attendee> sortHelper);
}