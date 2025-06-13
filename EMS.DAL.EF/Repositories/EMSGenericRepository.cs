using EMS.DAL.EF.Data;
using EMS.DAL.EF.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace EMS.DAL.EF.Repositories;

public abstract class EMSGenericRepository<TEntity> : IEMSGenericRepository<TEntity> where TEntity : class
{
    protected readonly EMSManagmentDbContext _context;
    protected readonly DbSet<TEntity> table;

    public EMSGenericRepository(EMSManagmentDbContext dbContext)
    {
        _context = dbContext;
        table = _context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await table.ToListAsync();
    }

    public virtual async Task<TEntity> GetByIdAsync(int id)
    {
        return await table.FindAsync(id);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity) + "Can't be null. [EMSGenericRepository.AddAsync()]");
        }
        var entityToAdd = await table.AddAsync(entity);
        return entityToAdd.Entity;
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity) + "Can't be null. [EMSGenericRepository.UpdateAsync()]");
        }
        table.Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteByIdAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        table.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity) + "Can't be null. [EMSGenericRepository.DeleteAsync()]");
        }
        table.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public IQueryable<TEntity> FindAll()
    {
        return _context.Set<TEntity>().AsNoTracking();
    }

    public async Task<IQueryable<TEntity>> FindByCondition(Expression<Func<TEntity, bool>> predicate)
    { 
        return await Task.Run(() => _context.Set<TEntity>().Where(predicate).AsNoTracking()); 
    }
}