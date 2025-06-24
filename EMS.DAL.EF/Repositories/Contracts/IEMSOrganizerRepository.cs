using EMS.DAL.EF.Entities;
using EMS.DAL.EF.Entities.HelpModels;
using EMS.DAL.EF.Helpers;
using Microsoft.AspNetCore.Identity;

namespace EMS.DAL.EF.Repositories.Contracts;

public interface IEMSOrganizerRepository : IEMSGenericRepository<Organizer>
{
    new IQueryable<Organizer> GetAllAsync();
    Task<IEnumerable<Organizer?>> GetByFullNameAsync(string name);
    Task<IEnumerable<Organizer?>> GetByEmailAsync(string email);
    Task<Organizer?> GetByPhoneNumberAsync(string phoneNumber);
    Task<PagedList<Organizer>> GetAllPaginatedAsync(OrganizerParameters parameters, ISortHelper<Organizer> sortHelper);
    Task<Organizer?> GetByIdAsync(string id);
    
    Task<(IdentityResult, Organizer)> CreateAsync(Organizer organizer, string password);
    new Task<IdentityResult> UpdateAsync(Organizer organizer);
    new Task<IdentityResult> DeleteAsync(Organizer organizer);
}