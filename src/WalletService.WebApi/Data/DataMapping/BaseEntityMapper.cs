using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using RiseOn.Domain.Record;

namespace WalletService.WebApi.Data.DataMapping;

public static class BaseEntityMapper
{
    public static void Mapper()
    {
        BsonClassMap.RegisterClassMap<Entity>(map =>
        {
            map.AutoMap();
            map.MapMember(x => x.CreateAt)
                .SetSerializer(new DateTimeSerializer(DateTimeKind.Local));
        });
    }
}