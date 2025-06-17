using EMS.DAL.EF.Entities;

namespace EMS.DAL.EF.Repositories.Contracts;

public interface IEMSAttendeeRepository : IEMSGenericRepository<Attendee>
{
    Task<IEnumerable<Attendee?>> GetByFirstNameAsync(string firstName);
    Task<IEnumerable<Attendee?>> GetByLastNameAsync(string lastName);
    Task<Attendee?> GetByEmailAsync(string email);
    Task<Attendee?> GetByPhoneNumberAsync(string phoneNumber);
}