using EMS.DAL.EF.Data;
using EMS.DAL.EF.Entities;
using EMS.DAL.EF.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace EMS.DAL.EF.Repositories;

public class EMSOrganizerRepository : EMSGenericRepository<Organizer>, IEMSOrganizerRepository
{
    public EMSOrganizerRepository(EMSManagmentDbContext dbContext) : base(dbContext)
    {
        
    }
    public async Task<IEnumerable<Organizer?>> GetByFullNameAsync(string fullName)
    {
        return await _context.Organizers.AsNoTracking()
            .Where(o => o.Name.Contains(fullName))
            .ToListAsync();
    }

    public async Task<IEnumerable<Organizer?>> GetByEmailAsync(string email)
    {
        return await _context.Organizers.AsNoTracking()
            .Where(o => o.Email == email)
            .ToListAsync();
    }

    public async Task<Organizer?> GetByPhoneNumberAsync(string phoneNumber)
    {
        return await _context.Organizers.AsNoTracking()
            .FirstOrDefaultAsync(o => o.PhoneNumber == phoneNumber);
    }
    
}