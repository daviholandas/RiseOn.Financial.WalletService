using WalletService.Tests.Fixtures;
using WalletService.WebApi.Domain.Enums;
using WalletService.WebApi.Models;
using WalletService.WebApi.Models.Inputs;

namespace WalletService.Tests.Integration.Application;

public class WebApiTests : IClassFixture<ApplicationFixture>
{
    private readonly HttpClient _applicationClient;

    public WebApiTests(ApplicationFixture applicationFixture)
        => this._applicationClient = applicationFixture.CreateClient();

    [Fact]
    public async Task ValidationFilter_WhenReceiveARequestInvalid_ReturnABadRequestStatus()
    {
        // Arrange
        var fakeWallet = new CommonWalletInput(string.Empty,
            10M, Currency.Real,
            WalletType.Money, null, 
            "Wallet test.");

        // Act
        var response = await this._applicationClient.PostAsJsonAsync("/api/wallet", fakeWallet);

        // Assert
        response.Should().Be400BadRequest()
            .And
            .Satisfy<IEnumerable<ErrorApplication>>(error => 
                error.Should().ContainSingle(x => x.ErrorMessage == "Name - 'Name' must not be empty."));
    }
}