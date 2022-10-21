using RiseOn.Domain;
using RiseOn.Domain.Record;
using WalletService.WebApi.Domain.Enums;
using WalletService.WebApi.Domain.Exceptions;

namespace WalletService.WebApi.Domain;

public record Wallet: Entity, IAggregateRoot
{
    public Wallet(string name, decimal amount,
        Currency currency, WalletType walletType, 
        decimal? limit,string? description = "")
    {
        this.Name = name ?? throw new ArgumentNullException(nameof(name));
        this.Description = description;
        this.Amount = amount;
        this.Currency = currency;
        this.WalletType = walletType;
        this.Limit = limit ?? amount;
    }

    public Wallet(string name, string description,
        decimal amount, Currency currency, 
        WalletType type, decimal? limit,
        int? dueDate, int? closingDay, 
        Flag? flag) : this(name, amount,
        currency, type,limit, description)
    {
        this.DueDate = dueDate;
        this.ClosingDay = closingDay;
        this.Flag = flag;
    }

    public string Name { get; private set; }

    public string? Description { get; set; }

    public decimal Amount { get; private set; }

    public Currency Currency { get; private set; }

    public WalletType WalletType { get; private set; }

    public decimal? Limit { get; private set; }

    public int? DueDate { get; private set; }

    public int? ClosingDay { get; private set; }

    public Flag? Flag { get; private set; }

    public decimal DecreaseAmount(decimal value)
    {
        if (this.Amount == this.Limit || value > this.Amount)
            throw new LimitExceededException();
        this.Amount -= value;
        return this.Amount;
    }
}