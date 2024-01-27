namespace Blogg.Services.interfaces;

public interface IBaseService<T> where T : class
{
    Task<T?> DeleteByIdAsync(int id, int loginUserId);
    Task<T?> GetByIdAsync(int id);
    Task<ICollection<T>> GetAsync(int pageNr, int pageSize);
}
