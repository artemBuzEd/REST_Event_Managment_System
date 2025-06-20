using EMS.DAL.EF.Entities;
using EMS.DAL.EF.Entities.HelpModels;
using EMS.DAL.EF.Helpers;

namespace EMS.DAL.EF.Repositories.Contracts;

public interface IEMSOrganizerRepository : IEMSGenericRepository<Organizer>
{
    Task<IEnumerable<Organizer?>> GetByFullNameAsync(string name);
    Task<IEnumerable<Organizer?>> GetByEmailAsync(string email);
    Task<Organizer?> GetByPhoneNumberAsync(string phoneNumber);
    Task<PagedList<Organizer>> GetAllPaginatedAsync(OrganizerParameters parameters, ISortHelper<Organizer> sortHelper);
}