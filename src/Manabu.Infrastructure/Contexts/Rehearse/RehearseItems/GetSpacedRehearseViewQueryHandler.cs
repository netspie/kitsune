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

        // totalRehearseItems
        var rehearseItemCollection = _mongoConnection.Database.GetCollection<RehearseItem>(RehearseItem.DefaultCollectionName);
        var rehearseItemHint = new BsonDocument(new Dictionary<string, object>
        {
            { nameof(RehearseItem.Owner), 1 },
            { nameof(RehearseItem.NextRehearseUtcTime), 1 },
            { nameof(RehearseItem.ItemType), 1 },
            { nameof(RehearseItem.Mode), 1 },
        });
        var rehearseItemFilter = Builders<RehearseItem>.Filter.Eq(nameof(RehearseItem.Owner), userId);
        var totalRehearseItems = rehearseItemCollection.CountDocuments(rehearseItemFilter, new CountOptions() { Hint = rehearseItemHint });

        // totalRehearseCollections
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

        // newItems
        rehearseItemFilter &= Builders<RehearseItem>.Filter.Eq(x => x.RepsInterval, 0);
        var totalNewItems = rehearseItemCollection.CountDocuments(rehearseItemFilter, new CountOptions() { Hint = rehearseItemHint });

        // itemsPlannedForTodayOnly
        var rehearseItemsForTodayExceptNewFilter = 
            Builders<RehearseItem>.Filter.Ne(x => x.RepsInterval, 0) &
            Builders<RehearseItem>.Filter.Lte(x => x.NextRehearseUtcTime, DateTime.UtcNow);
        var rehearseItemsForTodayExceptNew = rehearseItemCollection.CountDocuments(rehearseItemsForTodayExceptNewFilter, new CountOptions() { Hint = rehearseItemHint });

        // itemsTotalForToday
        var itemsTotalForToday = rehearseItemsForTodayExceptNew + totalNewItems;

        // failedRehearseItems
        var rehearseItemAsapCollection = _mongoConnection.Database.GetCollection<RehearseItemAsap>(RehearseItemAsap.DefaultCollectionName);
        var rehearseItemAsapFilter = Builders<RehearseItemAsap>.Filter.Eq(nameof(RehearseItemAsap.Owner), userId);
        var totalRehearseAsapItems = rehearseItemAsapCollection.CountDocuments(rehearseItemAsapFilter);

        return result.With(new GetRehearseViewQueryResponse(
            new((int) totalRehearseItems, 
                (int) totalRehearseCollections,
                (int) totalNewItems,
                (int) totalRehearseAsapItems,
                (int) rehearseItemsForTodayExceptNew,
                (int) itemsTotalForToday)));
    }
}
