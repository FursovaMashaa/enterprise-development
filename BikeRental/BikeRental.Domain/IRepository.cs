namespace BikeRental.Domain;

public interface IRepository<T, TId> where T : class
{
    public Task<T> Create(T entity);
    public Task<bool> Delete(TId id);
    public Task<T?> Read(TId id);
    public Task<IList<T>> ReadAll();
    public Task<T> Update(T entity);
}