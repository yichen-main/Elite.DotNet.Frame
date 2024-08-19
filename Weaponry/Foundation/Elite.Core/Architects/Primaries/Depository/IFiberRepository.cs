namespace Elite.Core.Architects.Primaries.Depository;
public interface IFiberRepository<T> where T : class
{
    Task<T?> GetAsync(string id);
    Task<IEnumerable<T>> GetAsync();
    Task AddAsync(T entity);
    Task PutAsync(string id, T entity, string[]? fields = default);
    Task ClearAsync(string id);
}