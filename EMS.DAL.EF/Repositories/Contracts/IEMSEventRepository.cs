using EMS.DAL.EF.Entities;
using EMS.DAL.EF.Entities.HelpModels;
using EMS.DAL.EF.Helpers;

namespace EMS.DAL.EF.Repositories.Contracts;

public interface IEMSEventRepository : IEMSGenericRepository<Event>
{
    Task<IEnumerable<Event?>> GetByNameAsync(string name);
    Task<IEnumerable<Event?>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<Event?> GetByIdWithVenueAndEventCategoryAndOrganizerAsync(int id);
    
    Task<IEnumerable<Event?>> GetAllByOrganizerIdAsync(string id);
    Task<PagedList<Event>> GetAllPaginatedAsync(EventParameters parameters, ISortHelper<Event> sortHelper);
 }