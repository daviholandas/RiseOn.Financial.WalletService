using RiseOn.Domain;
using RiseOn.Domain.Record;
using WalletService.WebApi.Domain.Enums;
using WalletService.WebApi.Domain.Exceptions;

namespace WalletService.WebApi.Domain;

public record Wallet: Entity, IAggregateRoot
{
    public Wallet(string name, decimal amount,
        Currency currency, WalletType type, 
        decimal? limit,string? description = "")
    {
        this.Name = name ?? throw new ArgumentNullException(nameof(name));
        this.Description = description;
        this.Amount = amount;
        this.Currency = currency;
        this.Type = type;
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

    public string Name { get; set; }

    public string? Description { get; set; }

    public decimal Amount { get; set; }

    public Currency Currency { get; }

    public WalletType Type { get; }

    public decimal? Limit { get; }

    public int? DueDate { get; }

    public int? ClosingDay { get; }

    public Flag? Flag { get; }

    public decimal DecreaseAmount(decimal value)
    {
        if (this.Amount == this.Limit || value > this.Amount)
            throw new LimitExceededException();
        this.Amount -= value;
        return this.Amount;
    }
}