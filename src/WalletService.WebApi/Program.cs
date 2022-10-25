using System.Reflection;
using WalletService.WebApi.Configurations;
using WalletService.WebApi.Domain;
using WalletService.WebApi.Domain.Repositories;
using WalletService.WebApi.Filters;
using WalletService.WebApi.Models;
using WalletService.WebApi.Models.Inputs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new ()
    {
        Title = "RiseOn.Financial.WalletService",
        Version = "v1"
    });
});

builder.Services.AddServicesCollection(builder.Configuration);

var app = builder.Build();

app.UseSwagger()
    .UseSwaggerUI();

app.UseHttpsRedirection()
    .UseAuthorization();


//Routes
var walletGroup = app.MapGroup("api/wallet")
    .WithTags("Wallets")
    .AddEndpointFilter<ValidationFilter>();

// Post: Create new common wallet.
walletGroup.MapPost("", async (IWalletRepository walletRepository,
        CommonWalletInput walletInput, CancellationToken cancellationToken) =>
    {
        var wallet = new Wallet(walletInput.Name, walletInput.Amount,
            walletInput.Currency, walletInput.WalletType,
            walletInput.Limit, walletInput.Description);
        await walletRepository.AddAsync(wallet, cancellationToken);
        return Results.Created($"wallet/{wallet.Id}", wallet);
    })
    .WithName("CreateCommonWallet")
    .Accepts<CommonWalletInput>("application/json")
    .Produces(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest, typeof(ErrorApplication))
    .Produces(StatusCodes.Status500InternalServerError);

walletGroup.MapPost("/creditcard", async (IWalletRepository walletRepository,
        CardWalletInput walletInput, CancellationToken cancellationToken) =>
    {
        var wallet = new Wallet(walletInput.Name, walletInput.Description,
            walletInput.Amount, walletInput.Currency, walletInput.WalletType,
            walletInput.Limit, walletInput.DueDate, walletInput.ClosingDay, walletInput.Flag);
        await walletRepository.AddAsync(wallet, cancellationToken);
        return Results.Created($"wallet/{wallet.Id}", wallet);
    })
    .WithName("CreateCreditCardWallet")
    .Accepts<CardWalletInput>("application/json")
    .Produces(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest, typeof(ErrorApplication))
    .Produces(StatusCodes.Status500InternalServerError);

walletGroup.MapGet("/", async (IWalletRepository walletRepository,
    CancellationToken cancellationToken) =>
    {
        var wallets = await walletRepository.GetAllAsync(cancellationToken);
        return Results.Ok(wallets.Select(x => new WalletCommon(x.Id, x.Name, x.Description,
            x.Currency.ToString(), x.WalletType.ToString(), x.Amount, x.Limit)));
    })
    .WithName("GetAllWallets")
    .WithSummary("Get all wallets.")
    .Produces(StatusCodes.Status200OK, typeof(IEnumerable<WalletCommon>))
    .Produces(StatusCodes.Status400BadRequest, typeof(ErrorApplication))
    .Produces(StatusCodes.Status500InternalServerError);

app.Run();