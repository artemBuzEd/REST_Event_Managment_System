using EMS.DAL.EF.Entities;

namespace EMS.DAL.EF.Repositories.Contracts;

public interface IEMSVenueRepository : IEMSGenericRepository<Venue>
{
    Task<Venue?> GetByNameAsync(string venueName);
    Task<Venue?> GetByAddressAsync(string address);
    Task<IEnumerable<Venue?>> GetByCityAsync(string city);
    Task<IEnumerable<Venue?>> GetByCountryAsync(string country);
    Task<IEnumerable<Venue?>> GetByCapacityAsync(int capacity);
}