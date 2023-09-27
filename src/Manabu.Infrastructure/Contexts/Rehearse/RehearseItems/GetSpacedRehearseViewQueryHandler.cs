using Corelibs.Basic.Auth;
using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using Corelibs.MongoDB;
using Manabu.Entities.Content.Users;
using Manabu.Entities.Rehearse.RehearseEntities;
using Manabu.Entities.Rehearse.RehearseItems;
using Manabu.UseCases.Rehearse.RehearseItems;
using Mediator;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Claims;

namespace Manabu.Infrastructure.CQRS.Rehearse.RehearseItems;

public class GetSpacedRehearseViewQueryHandler : IQueryHandler<GetRehearseViewQuery, Result<GetRehearseViewQueryResponse>>
{
    private readonly MongoConnection _mongoConnection;
    private readonly IAccessorAsync<ClaimsPrincipal> _userAccessor;

    public GetSpacedRehearseViewQueryHandler(
        MongoConnection mongoConnection,
        IAccessorAsync<ClaimsPrincipal> userAccessor)
    {
        _mongoConnection = mongoConnection;
        _userAccessor = userAccessor;
    }
    
    public async ValueTask<Result<GetRehearseViewQueryResponse>> Handle(
        GetRehearseViewQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetRehearseViewQueryResponse>.Success();

        var userId = await _userAccessor.GetUserID<UserId>();

        var rehearseItemCollection = _mongoConnection.Database.GetCollection<RehearseItem>(RehearseItem.DefaultCollectionName);
        var rehearseItemHint = new BsonDocument(new Dictionary<string, object>
        {
            { nameof(RehearseItem.Owner), 1 },
            { nameof(RehearseItem.RepsInterval), 1 },
            { nameof(RehearseItem.ItemType), 1 },
            { nameof(RehearseItem.Mode), 1 },
        });
        var rehearseItemFilter = Builders<RehearseItem>.Filter.Eq(nameof(RehearseItem.Owner), userId);
        var totalRehearseItems = rehearseItemCollection.CountDocuments(rehearseItemFilter, new CountOptions() { Hint = rehearseItemHint });

        var rehearsEntityCollection = _mongoConnection.Database.GetCollection<RehearseEntity>(RehearseEntity.DefaultCollectionName);
        var rehearsyHintEntity = new BsonDocument(new Dictionary<string, object>
        {
            { nameof(RehearseEntity.Owner), 1 },
            { nameof(RehearseEntity.IsItem), 1 }
        });
        var rehearsEntityFilter =
            Builders<RehearseEntity>.Filter.Eq(nameof(RehearseEntity.Owner), userId) &
            Builders<RehearseEntity>.Filter.Eq(nameof(RehearseEntity.IsItem), false);
        var totalRehearseCollections = rehearsEntityCollection.CountDocuments(rehearsEntityFilter, new CountOptions() { Hint = rehearsyHintEntity });

        return result.With(new GetRehearseViewQueryResponse(
            new((int) totalRehearseItems, (int) totalRehearseCollections)));
    }
}
