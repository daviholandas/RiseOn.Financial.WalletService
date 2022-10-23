using System.Reflection;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WalletService.WebApi.Data.DataMapping;
using WalletService.WebApi.Data.Repositories;
using WalletService.WebApi.Domain;
using WalletService.WebApi.Domain.Repositories;
using WalletService.WebApi.Models;

namespace WalletService.WebApi.Configurations;

public static class ServicesInjection
{
    public static IServiceCollection AddServicesCollection(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        serviceCollection.Configure<ApplicationSettings>(configuration.GetSection(nameof(ApplicationSettings)));

        var settings = configuration
            .GetSection(nameof(ApplicationSettings))
            .Get<ApplicationSettings>();

        serviceCollection.AddScoped<IMongoDatabase>(_ =>
            new MongoClient(settings?.DatabaseSettings.ConnectionString)
                .GetDatabase(settings?.DatabaseSettings.DatabaseName));

        RegisterDataMappers();

        serviceCollection.AddTransient<IWalletRepository, WalletRepository>();

        // MapperConfig
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(assembly);
        serviceCollection.AddSingleton(config);
        serviceCollection.AddScoped<IMapper, ServiceMapper>();

        //Validators
        serviceCollection.AddValidatorsFromAssembly(assembly);

        return serviceCollection;
    }

    private static void RegisterDataMappers()
    {
        WalletDataMapper.Mapper();
        BaseEntityMapper.Mapper();
    }
}