using EMS.DAL.EF.Entities;

namespace EMS.DAL.EF.Repositories.Contracts;

public interface IEMSEventCategoryRepository : IEMSGenericRepository<EventCategory>
{
    Task<IEnumerable<EventCategory?>> GetByNameAsync(string name);
}