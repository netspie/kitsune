using Corelibs.Basic.Blocks;
using Corelibs.MongoDB;
using Manabu.Entities.Content.WordMeanings;
using Manabu.Entities.Content.Words;
using Manabu.Entities.Rehearse.RehearseItems;
using Manabu.Entities.Shared;
using Manabu.UseCases.Content.Words;
using Mediator;
using MongoDB.Bson;
using MongoDB.Driver;
using static Manabu.Infrastructure.Contexts.RehearseItemLists.GetRehearseItemListQueryHandler;

namespace Manabu.Infrastructure.CQRS.Content.Lessons;

public class GetWordsQueryHandler : IQueryHandler<GetWordsQuery, Result<GetWordsQueryResponse>>
{
    private const int MaxItemLimit = 20;

    private readonly MongoConnection _mongoConnection;

    public GetWordsQueryHandler(
        MongoConnection mongoConnection)
    {
        _mongoConnection = mongoConnection;
    }

    public async ValueTask<Result<GetWordsQueryResponse>> Handle(GetWordsQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetWordsQueryResponse>.Success();

        var wordsCollection = _mongoConnection.Database.GetCollection<Word>(Word.DefaultCollectionName);
        var wordsFilter = Builders<Word>.Filter.Empty;

        var wordsHintDict = new Dictionary<string, object>();
        var wordsHint = new BsonDocument(wordsHintDict);

        //if (query.SortArgs)
        //wordsHint.Add

        var wordsProjection = Builders<Word>.Projection.Include(x => x.Id).Include(x => x.Value).Include(x => x.PartsOfSpeech).Include(x => x.Meanings);

        var words = await wordsCollection
            .Aggregate(new AggregateOptions() { })
            .Project(wordsProjection)
            .Lookup<WordMeaning, LookupResult>(
                foreignCollectionName: WordMeaning.DefaultCollectionName,
                localField: nameof(Word.Meanings),
                foreignField: "_id",
                @as: nameof(LookupResult.MeaningsX))
            .Limit(Math.Min(query.Limit, MaxItemLimit))
            .ToListAsync();

        return result.With(new GetWordsQueryResponse(
            new WordsDTO(
                Words: words.Select(w => new WordDTO(w.Id.Value, w.Value, w.MeaningsX.FirstOrDefault()?.Translations?.FirstOrDefault(), w.PartsOfSpeech.FirstOrDefault()?.Value)).ToArray(),
                Range: null,
                IsThereMoreWords: true)));
    }

    public record WordProjection(WordId Id, string Value, List<PartOfSpeech> PartsOfSpeech, WordMeaningId[] Meanings);
    public record LookupResult(WordId Id, string Value, List<PartOfSpeech> PartsOfSpeech, WordMeaningId[] Meanings, WordMeaning[] MeaningsX);
}
