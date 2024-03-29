﻿using WalletService.WebApi.Data.DataMapping;

namespace WalletService.Tests.Fixtures;

public class DatabaseFixture<T> : IAsyncLifetime
{
    private readonly bool _enableCollection;
    private readonly Fixture _fixture = new ();
    private readonly TestcontainerDatabase _testContainer;
    private IMongoCollection<T> _mongoCollection;
    private IMongoDatabase _mongoDatabase;

    public DatabaseFixture(bool enableCollection = true)
    {
        this._testContainer =  new TestcontainersBuilder<MongoDbTestcontainer>()
            .WithDatabase(new MongoDbTestcontainerConfiguration
            {
                Database = "WalletService",
                Username = "mongo",
                Password = "mongo"
            })
            .Build();
        this._enableCollection = enableCollection;
    }

    public IMongoDatabase MongoDatabase => this._mongoDatabase;

    public async Task InitializeAsync()
    {
       
        await this._testContainer.StartAsync()
            .ConfigureAwait(false);
        
        this._mongoDatabase = new MongoClient(this._testContainer.ConnectionString)
            .GetDatabase(this._testContainer.Database);

        if (this._enableCollection)
        {
            WalletDataMapper.Mapper();
            this._mongoCollection = this.MongoDatabase.GetCollection<T>(typeof(T).Name);
        }
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

    public async ValueTask<IEnumerable<T>> GetAllDocuments()
        => await (await this._mongoCollection.FindAsync(FilterDefinition<T>.Empty))
            .ToListAsync();

    public async Task ClearCollection()
        => await this._mongoCollection.DeleteManyAsync(x => true);
}