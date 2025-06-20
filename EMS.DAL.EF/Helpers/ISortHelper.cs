namespace EMS.DAL.EF.Helpers;

public interface ISortHelper<T>
{
    IQueryable<T> UseSort(IQueryable<T> entities, string? orderBy);
}