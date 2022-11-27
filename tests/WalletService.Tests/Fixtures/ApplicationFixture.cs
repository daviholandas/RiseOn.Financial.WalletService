using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using WalletService.WebApi.Domain;

namespace WalletService.Tests.Fixtures;

public class ApplicationFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly DatabaseFixture<Wallet> _databaseFixture;

    public ApplicationFixture()
        => this._databaseFixture = new DatabaseFixture<Wallet>(false);

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddScoped(_ => this._databaseFixture.MongoDatabase);
        });
    }

    public async Task InitializeAsync()
        => await this._databaseFixture.InitializeAsync();

    public async Task DisposeAsync()
        => await this._databaseFixture.DisposeAsync();
}