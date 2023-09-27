using Corelibs.Basic.Auth;
using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.DDD;
using Corelibs.Basic.Repository;
using Corelibs.MongoDB;
using Manabu.Entities.Content.Users;
using Manabu.Entities.Rehearse.RehearseItems;
using Manabu.Entities.Shared;
using Manabu.UseCases.Rehearse.RehearseItemLists;
using Mediator;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Numerics;
using System.Security.Claims;

namespace Manabu.Infrastructure.Contexts.RehearseItemLists;

public class GetRehearseItemListQueryHandler : IQueryHandler<GetSpacedRehearseItemListQuery, Result<GetSpacedRehearseItemListQueryResponse>>
{
    public const int SessionItemCount = 15;

    private readonly MongoConnection _mongoConnection;
    private readonly IAccessorAsync<ClaimsPrincipal> _userAccessor;

    public GetRehearseItemListQueryHandler(
        MongoConnection mongoConnection,
        IAccessorAsync<ClaimsPrincipal> userAccessor)
    {
        _mongoConnection = mongoConnection;
        _userAccessor = userAccessor;
    }

    public async ValueTask<Result<GetSpacedRehearseItemListQueryResponse>> Handle(
        GetSpacedRehearseItemListQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetSpacedRehearseItemListQueryResponse>.Success();

        var userId = await _userAccessor.GetUserID<UserId>();

        // ITEMS ASAP - Items to failed rehearse, which should be review as soon as possible
        var itemsAsapCollection = _mongoConnection.Database.GetCollection<RehearseItemAsap>(RehearseItemAsap.DefaultCollectionName);
        var itemsAsapFilter = Builders<RehearseItemAsap>.Filter.Eq("Owner", userId);
        var itemsAsapProjection = Builders<RehearseItemAsap>.Projection.Include(x => x.Id).Include(x => x.ItemId).Include(x => x.ItemType).Include(x => x.Mode); ;
        var itemsAsapDocs = await itemsAsapCollection
            .Find(itemsAsapFilter)
            .Project(itemsAsapProjection)
            .Limit(SessionItemCount)
            .ToListAsync();
        var itemsAsap = itemsAsapDocs.Select(doc => BsonSerializer.Deserialize<ItemProjection>(doc)).ToArray();

        // ITEMS - Items to review in general
        var itemsCollection = _mongoConnection.Database.GetCollection<RehearseItem>(RehearseItem.DefaultCollectionName);
        var itemsFilter = Builders<RehearseItem>.Filter.Eq("Owner", userId);
        if (!query.ItemTypes.IsNullOrEmpty())
        {
            var itemsTypeFilter = Builders<RehearseItem>.Filter.Eq(x => x.ItemType, new LearningItemType(query.ItemTypes[0].ToLower()));
            for (int i = 1; i < query.ItemTypes.Length; i++)
                itemsTypeFilter |= Builders<RehearseItem>.Filter.Eq(x => x.ItemType, new LearningItemType(query.ItemTypes[i].ToLower()));

            itemsFilter &= itemsTypeFilter;
        }
        if (!query.Modes.IsNullOrEmpty())
        {
            var modeFilter = Builders<RehearseItem>.Filter.Eq(x => x.Mode, new LearningMode(query.Modes[0].ToLower()));
            for (int i = 1; i < query.Modes.Length; i++)
                modeFilter |= Builders<RehearseItem>.Filter.Eq(x => x.Mode, new LearningMode(query.Modes[i].ToLower()));

            itemsFilter &= modeFilter;
        }
        var itemsProjection = Builders<RehearseItem>.Projection.Include(x => x.Id).Include(x => x.ItemId).Include(x => x.ItemType).Include(x => x.Mode);
        var itemsHint = new BsonDocument(new Dictionary<string, object>()
        {
            { nameof(RehearseItem.Owner), 1 },
            { nameof(RehearseItem.RepsInterval), 1 },
            { nameof(RehearseItem.ItemType), 1 },
            { nameof(RehearseItem.Mode), 1 },
        });
        var itemsDocs = itemsAsap.Length == SessionItemCount ?
            new() :
            await itemsCollection
                .Find(itemsFilter, new FindOptions() { Hint = itemsHint })
                .Project(itemsProjection)
                .Limit(SessionItemCount - itemsAsap.Length)
                .ToListAsync();
        var items = itemsDocs.Select(doc => BsonSerializer.Deserialize<ItemProjection>(doc)).ToArray();

        // ITEMS TOTAL
        var itemsTotal = new List<ItemProjection>(itemsAsap.Length + items.Length);
        itemsTotal.AddRange(itemsAsap);
        itemsTotal.AddRange(items);

        itemsTotal = itemsTotal.Shuffle().ToList();

        return result.With(new GetSpacedRehearseItemListQueryResponse(
            new FlashcardListDTO(itemsTotal.Select(i =>
                new FlashcardDTO(
                    i.Id.Value,
                    i.ItemId.Value,
                    i.ItemType.Value,
                    i.Mode.Value))
            .ToArray()
        )));
    }

    public record ItemProjection(RehearseItemId Id, LearningObjectId ItemId, LearningItemType ItemType, LearningMode Mode);
}
