using MongoDB.Driver;
using WalletService.WebApi.Domain;

namespace WalletService.WebApi.Data.Repositories;

public class WalletRepository : Repository<Wallet>
{
    public WalletRepository(IMongoDatabase database) : base(database) { }
}