using System.Linq.Expressions;
using FluentAssertions;
using MongoDB.Bson.Serialization;
using WalletService.Tests.Fixtures;
using WalletService.WebApi.Data.Repositories;
using WalletService.WebApi.Domain;
using WalletService.WebApi.Domain.Enums;
using WalletService.WebApi.Domain.Repositories;

namespace WalletService.Tests.Integration.Data;

public class WalletRepositoryTest : IClassFixture<DatabaseFixture<Wallet>>
{
    private readonly DatabaseFixture<Wallet> _databaseFixture;
    private readonly Fixture _fixture = new();
    private readonly IWalletRepository _walletRepository;

    public WalletRepositoryTest(DatabaseFixture<Wallet> databaseFixture)
    {
        this._databaseFixture = databaseFixture;
        this._walletRepository = new WalletRepository(this._databaseFixture.MongoDatabase);
    }

    [Fact]
    public async Task Add_GivenAnWalletEntity_ShouldSaveInDatabaseAfterGetIt()
    {
        // Arrange
        await this._databaseFixture.SeedDatabase(3);
        var walletTest = new Wallet("Test", 18M,
            Currency.Euro, WalletType.Money,
            null, "Testing...");
        var walletTestId = walletTest.Id;

        // Act
        this._walletRepository.Add(walletTest);
        var walletSaved = this._walletRepository.GetById(walletTestId);

        // Assert
        walletSaved.Should().BeEquivalentTo(walletTest, 
            x => x.Excluding(w => w.CreateAt));
    }

    [Fact]
    public async Task AddAsync_GivenAnWalletEntity_ShouldSaveInDatabaseAfterGetIt()
    {
        // Arrange
        var walletTest = new Wallet("Test2", "Testing..." ,18M,
            Currency.Libra , WalletType.Money,
            null, 10, 26, Flag.MasterCard);


        // Act
        await this._walletRepository.AddAsync(walletTest, CancellationToken.None);
        var walletSaved = await this._walletRepository.GetByIdAsync(walletTest.Id, CancellationToken.None);

        // Assert
        walletSaved.Should().BeEquivalentTo(walletTest, 
            x => x.Excluding(w => w.CreateAt));
    }

    [Fact]
    public async Task GetByFiltersAsync_GivenACollectionOfFilters_ShouldReturnAnCollectionOfWallets()
    {
        // Arrange
        var walletsCredit = this._fixture.Build<Wallet>()
            .With(x => x.WalletType, WalletType.CreditCard)
            .With(x => x.IsActive, true)
            .CreateMany(3);
        var walletsMoney = this._fixture.Build<Wallet>()
            .With(x => x.WalletType, WalletType.Money)
            .With(x => x.IsActive, true)
            .CreateMany(3);

        this._databaseFixture.SeedDatabase(walletsCredit);
        this._databaseFixture.SeedDatabase(walletsMoney);


        // Act
        await this._walletRepository.GetByFiltersAsync(
            new Dictionary<Expression<Func<Wallet, object>>, object>
            {
                {x => x.WalletType, WalletType.CreditCard},
                {x => x.IsActive, true}
            },
            CancellationToken.None);
        

        // Assert
        
    }
}