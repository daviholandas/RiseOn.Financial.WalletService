using MongoDB.Driver;
using WalletService.WebApi.Domain;
using WalletService.WebApi.Domain.Repositories;

namespace WalletService.WebApi.Data.Repositories;

public class WalletRepository : Repository<Wallet>, IWalletRepository
{
    public WalletRepository(IMongoDatabase database) : base(database) { }
}