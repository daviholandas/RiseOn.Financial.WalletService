using MongoDB.Driver;
using RiseOn.Domain.Record;
using WalletService.WebApi.Domain.Repositories;

namespace WalletService.WebApi.Data.Repositories;

public class Repository<T> : IRepository<T> where T : Entity
{
    private readonly IMongoCollection<T> _collection;

    public Repository(IMongoDatabase database)
    {
        this._collection = database.GetCollection<T>(nameof(T));
    }

    public void Add(T entity)
        => this._collection.InsertOne(entity);

    public async ValueTask AddAsync(T entity, CancellationToken cancellationToken = default)
        => await this._collection.InsertOneAsync(entity, cancellationToken: cancellationToken);

    public async ValueTask<IEnumerable<T>> GetAllAsync(T entity, CancellationToken cancellationToken = default)
        => (await this._collection.FindAsync(FilterDefinition<T>.Empty, cancellationToken: cancellationToken)).Current;

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => (await this._collection.FindAsync(
            Builders<T>.Filter.Eq(x => x.Id, id), 
            cancellationToken: cancellationToken)).Current.FirstOrDefault();
}