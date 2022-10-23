using Mapster;
using WalletService.WebApi.Configurations;
using WalletService.WebApi.Domain;
using WalletService.WebApi.Domain.Repositories;
using WalletService.WebApi.Filters;
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
    .WithTags("Wallet")
    .AddEndpointFilter<ValidationFilter>();


walletGroup.MapPost("/", async (IWalletRepository walletRepository,
    CommonWalletInput walletInput, CancellationToken cancellationToken) =>
{
    var wallet = walletInput.Adapt<Wallet>();
    await walletRepository.AddAsync(wallet, cancellationToken);
    return Results.Ok();
});

walletGroup.MapGet("/", () => Results.Ok());

app.Run();