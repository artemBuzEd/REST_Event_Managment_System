using EMS.DAL.EF.Data;
using EMS.DAL.EF.Entities;
using EMS.DAL.EF.Entities.HelpModels;
using EMS.DAL.EF.Helpers;
using EMS.DAL.EF.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace EMS.DAL.EF.Repositories;

public class EMSVenueRepository : EMSGenericRepository<Venue>, IEMSVenueRepository 
{
    public EMSVenueRepository(EMSDbContext dbContext) : base(dbContext)
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

    public async Task<IEnumerable<Venue?>> GetByCapacityRangeAsync(int startCapacity, int endCapacity)
    {
        return await _context.Venues.AsNoTracking()
            .Where(v => v.Capacity >= startCapacity && v.Capacity <= endCapacity)
            .ToListAsync();
    }

    public async Task<PagedList<Venue>> GetAllPaginatedAsync(VenueParameters parameters, ISortHelper<Venue> sortHelper)
    {
        var query = table.AsNoTracking();
        
        if(!string.IsNullOrWhiteSpace(parameters.Name))
            query = query.Where(e => e.Name.ToLower().Contains(parameters.Name.ToLower()));
        
        if(parameters.MinCapacity >= 1 && parameters.MaxCapacity >= parameters.MinCapacity)
            query = query.Where(e => e.Capacity >= parameters.MinCapacity);
        
        if(parameters.MaxCapacity >= parameters.MinCapacity && parameters.MaxCapacity > 0)
            query = query.Where(e => e.Capacity <= parameters.MaxCapacity);
        
        query = sortHelper.UseSort(query, parameters.OrderBy);
        
        return await PagedList<Venue>.ToPagedListAsync(query.AsNoTracking(), parameters.PageNumber, parameters.PageSize);
    }
}