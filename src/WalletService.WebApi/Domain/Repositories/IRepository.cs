namespace WalletService.WebApi.Domain.Repositories;

public interface IRepository<T>
{
    void Add(T entity);

    ValueTask AddAsync(T entity, CancellationToken cancellationToken);

    ValueTask<IEnumerable<T>> GetAllAsync(T entity, CancellationToken cancellationToken);

    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}