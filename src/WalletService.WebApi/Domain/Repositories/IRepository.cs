using System.Linq.Expressions;

namespace WalletService.WebApi.Domain.Repositories;

public interface IRepository<T>
{
    void Add(T entity);

    ValueTask AddAsync(T entity, CancellationToken cancellationToken);

    ValueTask<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);

    T? GetById(Guid id);

    ValueTask<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    ValueTask<IEnumerable<T>> GetByFiltersAsync(IDictionary<Expression<Func<T, object>>, object> filters, CancellationToken cancellationToken);
}