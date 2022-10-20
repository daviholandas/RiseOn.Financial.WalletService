using System.Linq.Expressions;
using WalletService.WebApi.Data.DataMapping;

namespace WalletService.Tests.Fixtures;

public class DatabaseFixture<T> : IAsyncLifetime
{
    private readonly TestcontainerDatabase _testContainer;
    private IMongoDatabase _mongoDatabase;
    private IMongoCollection<T> _mongoCollection;
    private readonly Fixture _fixture = new ();

    public DatabaseFixture()
    {
        this._testContainer =  new TestcontainersBuilder<MongoDbTestcontainer>()
            .WithDatabase(new MongoDbTestcontainerConfiguration
            {
                Database = "WalletService",
                Username = "mongo",
                Password = "mongo"
            })
            .Build();
    }

    public IMongoDatabase MongoDatabase => this._mongoDatabase;

    public async Task InitializeAsync()
    {
        await this._testContainer.StartAsync()
            .ConfigureAwait(false);
        this._mongoDatabase = new MongoClient(this._testContainer.ConnectionString)
            .GetDatabase(this._testContainer.Database);
        this._mongoCollection = this.MongoDatabase.GetCollection<T>(typeof(T).Name);
    }

    public async Task DisposeAsync()
        => await this._testContainer.DisposeAsync()
            .ConfigureAwait(false);

    public async Task SeedDatabase(int quantity = 1)
    {
        var seeds = this._fixture.CreateMany<T>(quantity);

        await this._mongoCollection.InsertManyAsync(seeds);
    }

    public async Task SeedDatabase(IEnumerable<T> seeds)
        => await this._mongoCollection.InsertManyAsync(seeds);

    public async ValueTask<T> GetDocument(
        Expression<Func<T, object>> criteria, 
        object propertyValue)
    {
        var cursor = await this._mongoCollection
            .FindAsync(Builders<T>.Filter.Eq(criteria, propertyValue));
        return cursor.FirstOrDefault();
    }
}