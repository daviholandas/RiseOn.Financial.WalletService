namespace WalletService.WebApi.Models;

public record struct WalletCommon(Guid Id, string Name, 
    string Description, string Currency, 
    string WalletType, decimal Amount,
    decimal? Limit);