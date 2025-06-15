using EMS.DAL.EF.Data;
using EMS.DAL.EF.Entities;
using EMS.DAL.EF.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace EMS.DAL.EF.Repositories;

public class EMSVenueRepository : EMSGenericRepository<Venue>, IEMSVenueRepository 
{
    public EMSVenueRepository(EMSManagmentDbContext dbContext) : base(dbContext)
    {
        
    }
    public async Task<Venue?> GetByNameAsync(string name)
    {
        return await _context.Venues.AsNoTracking()
            .FirstOrDefaultAsync(v => v.Name.ToLower() == name.ToLower());
    }

    public async Task<Venue?> GetByAddressAsync(string address)
    {
        return await _context.Venues.AsNoTracking()
            .FirstOrDefaultAsync(v => v.Address.ToLower() == address.ToLower());
    }

    public async Task<IEnumerable<Venue?>> GetByCityAsync(string city)
    {
        return await _context.Venues.AsNoTracking()
            .Where(v => v.City.ToLower() == city.ToLower())
            .ToListAsync();
    }

    public async Task<IEnumerable<Venue?>> GetByCountryAsync(string country)
    {
        return await _context.Venues.AsNoTracking()
            .Where(v => v.Country.ToLower() == country.ToLower())
            .ToListAsync();
    }

    public async Task<IEnumerable<Venue?>> GetByCapacityAsync(int capacity)
    {
        return await _context.Venues.AsNoTracking()
            .Where(v => v.Capacity == capacity)
            .ToListAsync();
    }
}