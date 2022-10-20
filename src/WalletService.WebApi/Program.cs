using WalletService.WebApi.Configurations;
using WalletService.WebApi.Domain;
using WalletService.WebApi.Domain.Enums;
using WalletService.WebApi.Domain.Repositories;
using WalletService.WebApi.Models.Inputs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddServicesCollection(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("api/wallet", async (CommonWallet wallet, IWalletRepository repository) =>
{
    var walletEntity = new Wallet(wallet.Name, wallet.Amount, Currency.Euro, WalletType.Money, null, wallet.Description);
        await repository.AddAsync(walletEntity, CancellationToken.None);
        return Results.Created($"/wallet/{walletEntity.Id}", wallet);
});

app.MapGet("api/wallet", async (IWalletRepository repository) => await repository.GetAllAsync(CancellationToken.None));
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
