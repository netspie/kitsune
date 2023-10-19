using Corelibs.Basic.Blocks;
using Corelibs.MongoDB;
using Manabu.Entities.Content.WordMeanings;
using Manabu.Entities.Content.Words;
using Manabu.UseCases.Content.Words;
using Mediator;
using MongoDB.Bson;
using MongoDB.Driver;

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

        var words = await wordsCollection
            .Aggregate(new AggregateOptions() { })
            .Lookup<WordMeaning, LookupResult>(
                foreignCollectionName: WordMeaning.DefaultCollectionName,
                localField: nameof(Word.Meanings),
                foreignField: nameof(WordMeaning.Id),
                @as: nameof(LookupResult.Meanings))
            .Limit(Math.Min(query.Limit, MaxItemLimit))
            .ToListAsync();
        
        return result.With(new GetWordsQueryResponse(
            new WordsDTO(
                Words: words.Select(w => new WordDTO(w.Word.Id.Value, w.Word.Value, w.Meanings[0].Translations[0], w.Word.PartsOfSpeech[0].Value)).ToArray(),
                Range: null,
                IsThereMoreWords: true)));
    }

    public record LookupResult(Word Word, WordMeaning[] Meanings);
}
