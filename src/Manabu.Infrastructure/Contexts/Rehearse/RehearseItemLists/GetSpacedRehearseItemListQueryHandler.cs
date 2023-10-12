using Corelibs.Basic.Auth;
using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
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
using System.Linq.Expressions;
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

        var nowUtcTime = DateTime.UtcNow;
        var nowUtcTimeTicks = nowUtcTime.Ticks;

        // ITEMS - Items to review in general
        var itemsCollection = _mongoConnection.Database.GetCollection<RehearseItem>(RehearseItem.DefaultCollectionName);
        var itemsFilter = Builders<RehearseItem>.Filter.Eq("Owner", userId);
        if (query.DayInterval is not null)
            itemsFilter &= Builders<RehearseItem>.Filter.Eq(x => x.RepsInterval, query.DayInterval.Value);
        else
        {
            var currentTime = DateTime.UtcNow;
            long ticksInADay = TimeSpan.FromDays(1).Ticks;
            itemsFilter &= GetRehearsedTimeFilter();
        }
        if (!query.ItemTypes.IsNullOrEmpty())
            itemsFilter &= GetFilter<RehearseItem, string, LearningItemType>(x => x.ItemType, query.ItemTypes, m => new LearningItemType(m.ToLower()));
        if (!query.Modes.IsNullOrEmpty())
            itemsFilter &= GetFilter<RehearseItem, string, LearningMode>(x => x.Mode, query.Modes, m => new LearningMode(m.ToLower()));

        var itemsProjection = Builders<RehearseItem>.Projection.Include(x => x.Id).Include(x => x.ItemId).Include(x => x.ItemType).Include(x => x.Mode);
        var itemsHint = new BsonDocument(new Dictionary<string, object>()
        {
            { nameof(RehearseItem.Owner), 1 },
            { nameof(RehearseItem.NextRehearseUtcTime), 1 },
            { nameof(RehearseItem.ItemType), 1 },
            { nameof(RehearseItem.Mode), 1 },
        });

        var time = DateTime.UtcNow;
        var time2 = time + TimeSpan.FromDays(1) * 3;

        var itemsDocs = itemsAsap.Length == SessionItemCount ?
            new() :
                query.Random ?
                    await itemsCollection
                        .Aggregate(new AggregateOptions() { Hint = itemsHint })
                        .Match(itemsFilter)
                        .AppendStage<RehearseItem>($@"{{ $sample: {{ size: {SessionItemCount - itemsAsap.Length} }} }}")
                        .Project(itemsProjection)
                        .ToListAsync() :
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

    private static FilterDefinition<TEntity> GetFilter<TEntity, TFieldIn, TFieldOut>(
            Expression<Func<TEntity, TFieldOut>> field, TFieldIn[] array, Func<TFieldIn, TFieldOut> transform)
    {
        var filter = Builders<TEntity>.Filter.Eq(field, transform(array[0]));
        for (int i = 1; i < array.Length; i++)
            filter |= Builders<TEntity>.Filter.Eq(field, transform(array[i]));

        return filter;
    }

    private BsonDocument GetRehearsedTimeFilter()
    {
        var currentTime = DateTime.UtcNow;
        long ticksInADay = TimeSpan.FromDays(1).Ticks;

        return new BsonDocument(new Dictionary<string, object>()
        {
            {
                "$expr", new BsonDocument
                {
                    {
                        "$gt", new BsonArray
                        {
                            currentTime,
                            new BsonDocument
                            {
                                {
                                    "$add", new BsonArray
                                    {
                                        "$LastRehearsedUtcTime",
                                        new BsonDocument
                                        {
                                            {
                                                "$multiply", new BsonArray // TO DO: FIX the problem with always offsetting time, make possible to practice anytime in the day. - maybe add another last time rehearsed just for checks as in reps counting
                                                {
                                                    "$RepsInterval",
                                                    ticksInADay
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        });
    }

    public record ItemProjection(LearningObjectId Id, LearningObjectId ItemId, LearningItemType ItemType, LearningMode Mode);
}
