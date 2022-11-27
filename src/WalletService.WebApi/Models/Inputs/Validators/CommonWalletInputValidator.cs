using FluentValidation;
using WalletService.WebApi.Domain.Enums;

namespace WalletService.WebApi.Models.Inputs.Validators;

public class CommonWalletInputValidator : AbstractValidator<CommonWalletInput>
{
    public CommonWalletInputValidator()
    {
        this.RuleLevelCascadeMode = CascadeMode.Continue;

        this.RuleFor(x => x.Name)
            .NotEmpty();
        this.RuleFor(x => x.Amount)
            .GreaterThan(0);
        this.RuleFor(x => x.WalletType)
            .NotEqual(WalletType.CreditCard);
    }
}