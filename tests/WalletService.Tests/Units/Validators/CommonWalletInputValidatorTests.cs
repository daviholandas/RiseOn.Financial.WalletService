using System.ComponentModel;
using WalletService.WebApi.Domain.Enums;
using WalletService.WebApi.Models.Inputs;
using WalletService.WebApi.Models.Inputs.Validators;

namespace WalletService.Tests.Units.Validators;

public class CommonWalletInputValidatorTests
{
    private readonly IFixture _fixture = new Fixture();

    [Fact]
    public void NameValidation_GivenACommonWalletInput_ShouldReturnAnInValidStatus()
    {
        // Arrange
        var commonWallet = this._fixture.Build<CommonWalletInput>()
            .With(x => x.Name, string.Empty)
            .Create();
        var validator = new CommonWalletInputValidator();

        // Act
        var result = validator.Validate(commonWallet);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void AmountLessThanOrEqualZero_GivenACommonWalletInput_ShouldReturnAnInValidStatus()
    {
        // Arrange
        var commonWallet = this._fixture.Build<CommonWalletInput>()
            .With(x => x.Amount, 0)
            .With(x => x.WalletType, WalletType.Money)
            .Create();
        var validator = new CommonWalletInputValidator();

        // Act
        var result = validator.Validate(commonWallet);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void WalletTypeEqualCreditCard_GivenACommonWalletInput_ShouldReturnAnInValidStatus()
    {
        // Arrange
        var commonWallet = this._fixture.Build<CommonWalletInput>()
            .With(x => x.WalletType, WalletType.CreditCard)
            .Create();
        var validator = new CommonWalletInputValidator();

        // Act
        var result = validator.Validate(commonWallet);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Valid_GivenACommonWalletInput_ShouldReturnAValidStatus()
    {
        // Arrange
        var commonWallet = this._fixture.Build<CommonWalletInput>()
            .With(x => x.WalletType, WalletType.Pix)
            .Create();
        var validator = new CommonWalletInputValidator();

        // Act
        var result = validator.Validate(commonWallet);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}