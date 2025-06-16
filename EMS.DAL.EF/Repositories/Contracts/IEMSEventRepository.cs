using EMS.DAL.EF.Entities;

namespace EMS.DAL.EF.Repositories.Contracts;

public interface IEMSEventRepository : IEMSGenericRepository<Event>
{
    Task<IEnumerable<Event?>> GetByNameAsync(string name);
    Task<IEnumerable<Event?>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<Event?> GetByIdWithVenueAndEventCategoryAndOrganizerAsync(int id);
}