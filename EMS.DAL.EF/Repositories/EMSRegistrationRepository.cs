using EMS.DAL.EF.Data;
using EMS.DAL.EF.Entities;
using EMS.DAL.EF.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace EMS.DAL.EF.Repositories;

public class EMSRegistrationRepository : EMSGenericRepository<Registration>, IEMSRegistrationRepository
{
    public EMSRegistrationRepository(EMSManagmentDbContext dbContext) : base(dbContext)
    {
        
    }

    public async Task<IEnumerable<Registration?>> GetByRegistrationDateAsync(DateTime registrationDate)
    {
        var nextDay = registrationDate.AddDays(1);
        
        return await _context.Registrations.AsNoTracking()
            .Where(r => r.RegistrationDate >= registrationDate.Date && r.RegistrationDate < nextDay)
            .ToListAsync();
    }

    public async Task<IEnumerable<Registration?>> GetByStatusAsync(string status)
    {
        return await _context.Registrations.AsNoTracking()
            .Where(r => r.Status == status)
            .ToListAsync();
    }
}