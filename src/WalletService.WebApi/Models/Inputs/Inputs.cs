using WalletService.WebApi.Domain.Enums;

namespace WalletService.WebApi.Models.Inputs;

public record CommonWalletInput(
    string Name, decimal Amount, 
    Currency Currency, WalletType WalletType,
    decimal? Limit, string Description) : IInput;
    
public record CardWalletInput(
    string Name, string Description,
    decimal Amount, Currency Currency, 
    WalletType WalletType, decimal? Limit,
    int? DueDate, int? ClosingDay, 
    Flag? Flag) : IInput;