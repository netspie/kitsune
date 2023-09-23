using Corelibs.Basic.Auth;
using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using Corelibs.MongoDB;
using Manabu.Entities.Content.Users;
using Manabu.Entities.Rehearse.RehearseItems;
using Manabu.UseCases.Rehearse.RehearseItemLists;
using Mediator;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Claims;

namespace Manabu.Infrastructure.CQRS.Rehearse.RehearseItems;

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

        var userId = await _userAccessor.GetUserID<UserId>();

        //var filter = 
        //    Builders<RehearseItem>.Filter.Eq(x => x.Owner, userId) & 
        //    Builders<RehearseItem>.Filter.Eq(x => x.ItemType, query.ItemType) &
        //    Builders<RehearseItem>.Filter.Eq(x => x.Mode, query.Mode);

        var sortDefinition = Builders<RehearseItem>.Sort.Ascending(_ => _.RepsInterval);

        var min = new BsonDocument("$min", "{ item: 'apple', type: 'jonagold' }");
        var hint = new BsonDocument("$hint", "{ item: 'apple', type: 'jonagold' }");
        //await rehearseItemCollection.Find(filter, new FindOptions { Min = min, Hint = hint }).ToListAsync();

        //var results = await rehearseItemCollection.Find(filter).Sort(sortDefinition).Limit(20).ToListAsync();
        //var filterX = Builders<TEntity>.Filter.In(x => x.Id, ids);

        return result;
    }
}
