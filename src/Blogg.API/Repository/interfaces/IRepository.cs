namespace Blogg.Repository.interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> UpdateAsync(int id, T entity);
    Task<T?> DeleteByIdAsync(int id);
    Task<T?> GetByIdAsync(int id);
    Task<ICollection<T>> GetAsync(int pageNr, int pageSize);
}
