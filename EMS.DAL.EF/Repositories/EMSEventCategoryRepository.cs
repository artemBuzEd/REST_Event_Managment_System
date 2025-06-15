using System.Collections;
using EMS.DAL.EF.Data;
using EMS.DAL.EF.Entities;
using EMS.DAL.EF.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace EMS.DAL.EF.Repositories;

public class EMSEventCategoryRepository : EMSGenericRepository<EventCategory>, IEMSEventCategoryRepository
{
    public EMSEventCategoryRepository(EMSManagmentDbContext dbContext) : base(dbContext)
    {
        
    }

    public async Task<IEnumerable<EventCategory?>> GetByNameAsync(string name)
    {
        string search = name.ToLower();
        return await _context.EventCategories.AsNoTracking()
            .Where(c => c.Name.ToLower().Contains(search))
            .ToListAsync();
    }
}