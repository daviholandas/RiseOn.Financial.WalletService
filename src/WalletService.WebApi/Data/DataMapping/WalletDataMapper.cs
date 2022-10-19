using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using WalletService.WebApi.Domain;
using WalletService.WebApi.Domain.Enums;

namespace WalletService.WebApi.Data.DataMapping;

public static class WalletDataMapper
{
    public static void Mapper()
    {
        BsonClassMap.RegisterClassMap<Wallet>(map =>
        {
            map.AutoMap();
            map.MapMember(x => x.Currency)
                .SetSerializer(new EnumSerializer<Currency>(BsonType.String));
            map.MapMember(x => x.Flag)
                .SetSerializer(new NullableSerializer<Flag>(new EnumSerializer<Flag>(BsonType.String)));
            map.MapMember(x => x.Type)
                .SetSerializer(new EnumSerializer<WalletType>());
            map.MapCreator(w => new Wallet(w.Name, w.Amount, w.Currency, w.Type, w.Limit, w.Description));
            map.MapMember(x => x.Limit)
                .SetSerializer(new NullableSerializer<decimal>());
        });
    }
}