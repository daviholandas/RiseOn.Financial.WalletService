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
    public void Add_GivenAnWalletEntity_ShouldSaveInDatabaseAfterGetIt()
    {
        // Arrange
        var walletTest = new Wallet("Test", 18M,
            Currency.Euro, WalletType.Money,
            null, "Testing...");

        // Act
        this._walletRepository.Add(walletTest);

        var result = this._databaseFixture
            .GetDocument(wallet => wallet.Id == wallet.Id,
                walletTest.Id);

        // Assert
        
    }
}