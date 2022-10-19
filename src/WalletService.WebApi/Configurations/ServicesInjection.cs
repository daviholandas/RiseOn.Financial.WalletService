using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WalletService.WebApi.Data.DataMapping;
using WalletService.WebApi.Data.Repositories;
using WalletService.WebApi.Domain.Repositories;
using WalletService.WebApi.Models;

namespace WalletService.WebApi.Configurations;

public static class ServicesInjection
{
    public static IServiceCollection AddServicesCollection(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.Configure<ApplicationSettings>(configuration.GetSection(nameof(ApplicationSettings)));

        var settings = configuration
            .GetSection(nameof(ApplicationSettings))
            .Get<ApplicationSettings>();

        serviceCollection.AddScoped<IMongoDatabase>(_ =>
            new MongoClient(settings?.DatabaseSettings.ConnectionString)
                .GetDatabase(settings?.DatabaseSettings.CollectionName));

        WalletDataMapper.Mapper();

        serviceCollection.AddTransient<IWalletRepository, WalletRepository>();

        return serviceCollection;
    }
}