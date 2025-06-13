using EMS.DAL.EF.Entities;

namespace EMS.DAL.EF.Repositories.Contracts;

public interface IEMSOrganizerRepository : IEMSGenericRepository<Organizer>
{
    Task<IEnumerable<Organizer?>> GetByFullNameAsync(string name);
    Task<IEnumerable<Organizer?>> GetByEmailAsync(string email);
    Task<Organizer?> GetByPhoneNumberAsync(string phoneNumber);
}