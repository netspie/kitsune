using Corelibs.Basic.Auth;
using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using Corelibs.MongoDB;
using Manabu.Entities.Rehearse.RehearseContainers;
using Manabu.Entities.RehearseItems;
using Mediator;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Claims;
using Manabu.Entities.Content.Users;
using Manabu.UseCases.Rehearse.RehearseItemLists;

namespace Manabu.Infrastructure.CQRS.RehearseItems;

public class GetRehearseItemListQueryHandler : IQueryHandler<GetSpacedRehearseItemListQuery, Result<GetRehearseItemListQueryResponse>>
{
    private readonly MongoConnection _mongoConnection;
    private readonly IAccessorAsync<ClaimsPrincipal> _userAccessor;

    public GetRehearseItemListQueryHandler(
       MongoConnection mongoConnection,
        IAccessorAsync<ClaimsPrincipal> userAccessor)
    {
        _mongoConnection = mongoConnection;
        _userAccessor = userAccessor;
    }
        
    public async ValueTask<Result<GetRehearseItemListQueryResponse>> Handle(
        GetSpacedRehearseItemListQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetRehearseItemListQueryResponse>.Success();

        var rehearseItemCollection = _mongoConnection.Database.GetCollection<RehearseItem>(RehearseItem.DefaultCollectionName);
        var rehearseContainerCollection = _mongoConnection.Database.GetCollection<RehearseContainer>(RehearseContainer.DefaultCollectionName);

        var userId = await _userAccessor.GetUserID<UserId>();

        //var filter = 
        //    Builders<RehearseItem>.Filter.Eq(x => x.Owner, userId) & 
        //    Builders<RehearseItem>.Filter.Eq(x => x.ItemType, query.ItemType) &
        //    Builders<RehearseItem>.Filter.Eq(x => x.Mode, query.Mode);

        var sortDefinition = Builders<RehearseItem>.Sort.Ascending(_ => _.SessionsToNextRehearse);

        var min = new BsonDocument("$min", "{ item: 'apple', type: 'jonagold' }");
        var hint = new BsonDocument("$hint", "{ item: 'apple', type: 'jonagold' }");
        //await rehearseItemCollection.Find(filter, new FindOptions { Min = min, Hint = hint }).ToListAsync();

        //var results = await rehearseItemCollection.Find(filter).Sort(sortDefinition).Limit(20).ToListAsync();
        //var filterX = Builders<TEntity>.Filter.In(x => x.Id, ids);

        return result;
    }
}
