using System.Linq.Expressions;
using MongoDB.Driver;
using RiseOn.Domain.Record;
using WalletService.WebApi.Domain.Repositories;

namespace WalletService.WebApi.Data.Repositories;

public class Repository<T> : IRepository<T> where T : Entity
{
    private readonly IMongoCollection<T> _collection;

    public Repository(IMongoDatabase database)
    {
        this._collection = database.GetCollection<T>(typeof(T).Name);
    }

    public void Add(T entity)
        => this._collection.InsertOne(entity);

    public async ValueTask AddAsync(T entity, CancellationToken cancellationToken = default)
        => await this._collection.InsertOneAsync(entity, cancellationToken: cancellationToken);

    public async ValueTask<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await (await this._collection.FindAsync(x => true, cancellationToken: cancellationToken))
            .ToListAsync(cancellationToken);
    }

    public async ValueTask<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await (await this._collection.FindAsync(
            Builders<T>.Filter.Eq(x => x.Id, id), 
            cancellationToken: cancellationToken))
            .FirstOrDefaultAsync(cancellationToken);

    public async ValueTask<IEnumerable<T>> GetByFiltersAsync(IDictionary<Expression<Func<T, object>>, object> filters,
        CancellationToken cancellationToken)
    {
        var builderFilter = Builders<T>.Filter;
        var initialFilter = filters
            .Aggregate(builderFilter.Empty, 
                (current, filter) => current & builderFilter.Eq(filter.Key, filter.Value));
        return await this._collection.FindSync(initialFilter, null, cancellationToken).ToListAsync(cancellationToken);
    }

    public T? GetById(Guid id)
        => this._collection.FindSync(
            Builders<T>.Filter.Eq(x => x.Id, id)).FirstOrDefault();
}