using System.Linq.Expressions;
namespace EMS.DAL.EF.Repositories.Contracts;

public interface IEMSGenericRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> GetByIdAsync(int id);
    Task<TEntity> AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteByIdAsync(int id);
    Task DeleteAsync(TEntity entity);
    
    IQueryable<TEntity> FindAll();
    Task<IQueryable<TEntity>> FindByCondition(Expression<Func<TEntity, bool>> predicate);
}