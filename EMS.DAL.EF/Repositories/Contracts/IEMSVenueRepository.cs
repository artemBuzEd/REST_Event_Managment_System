using EMS.DAL.EF.Entities;
using EMS.DAL.EF.Entities.HelpModels;
using EMS.DAL.EF.Helpers;

namespace EMS.DAL.EF.Repositories.Contracts;

public interface IEMSVenueRepository : IEMSGenericRepository<Venue>
{
    Task<Venue?> GetByNameAsync(string venueName);
    Task<Venue?> GetByAddressAsync(string address);
    Task<IEnumerable<Venue?>> GetByCityAsync(string city);
    Task<IEnumerable<Venue?>> GetByCountryAsync(string country);
    Task<IEnumerable<Venue?>> GetByCapacityRangeAsync(int startCapacity, int endCapacity);
    Task<PagedList<Venue>> GetAllPaginatedAsync(VenueParameters parameters, ISortHelper<Venue> sortHelper);
}