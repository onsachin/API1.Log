namespace WebApiOne.Api.Operations;

public interface IOperationHandler<T>
{
    Task<T> GetByIdAsync(T entity);
    Task<T?> GetByIdAsync(object id);
    Task<T?> GetByEmailAsync(string email);
    Task<IList<T>> GetByQueryAsync(string query);
    Task<IList<T>> GetAllAsync();
    Task<T> Insert(T entity);
    Task<T> Update(T entity);
    Task<T> Delete(T entity);
}
