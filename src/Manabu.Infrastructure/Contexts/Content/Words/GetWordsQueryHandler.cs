using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.MongoDB;
using Manabu.Entities.Content.WordMeanings;
using Manabu.Entities.Content.Words;
using Manabu.UseCases.Content.Words;
using Mediator;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using MongoDB.Driver.Search;

namespace Manabu.Infrastructure.CQRS.Content.Words;

using SearchDef = (string IndexName, SearchDefinition<Word> Definition);

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

        var range = query.Range ?? new RangeArg(0, MaxItemLimit);
        var limit = Math.Min(range.Limit, MaxItemLimit);

        var filter = Builders<Word>.Filter.Empty;
        List<SearchDef> searches = [];
        List<string> sorts = [];
        foreach (var arg in query.Modifiers.ToArrayOrEmpty())
        {
            if (arg.IsSort)
            {
                if (arg.Field == nameof(Word))
                    sorts.Add($"{{Value:{arg.Order}}}");

                if (arg.Field == "Part Of Speech")
                    sorts.Add($"{{PartsOfSpeech:{arg.Order}}}");
            }
            else
            {
                if (arg.Field.IsNullOrEmpty() ||
                    arg.Value.IsNullOrEmpty())
                    continue;

                if (arg.Field == "Part Of Speech")
                {
                    filter &= Builders<Word>.Filter.Eq(x => x.PartsOfSpeech[0].Value, arg.Value.ToLower());
                }
                else
                if (arg.Field == nameof(Word) && MongoConfig.IsAtlas)
                {
                    //filter &= Builders<Word>.Filter.Text($"{arg.Value}~");
                    searches.Add(new("searchWordsByValue", Builders<Word>.Search.Regex(x => x.Value, $"(.*){arg.Value}(.*)")));
                }
            }
        }

        var wordsProjection = Builders<Word>.Projection.Include(x => x.Id).Include(x => x.Value).Include(x => x.PartsOfSpeech).Include(x => x.Meanings);
        var totalCount = await wordsCollection.CountDocumentsAsync(filter);
        
        var aggregate = wordsCollection.Aggregate(new AggregateOptions());
        foreach (var search in searches)
            aggregate = aggregate.Search(search.Definition, new SearchOptions<Word>() { IndexName = search.IndexName });

        aggregate = aggregate.Match(filter);
        foreach (var sort in sorts)
            aggregate = aggregate.Sort(sort);

        var words = await aggregate
            .Project(wordsProjection)
            .Skip(range.Start)
            .Lookup<WordMeaning, LookupResult>(
                foreignCollectionName: WordMeaning.DefaultCollectionName,
                localField: nameof(Word.Meanings),
                foreignField: "_id",
                @as: nameof(LookupResult.MeaningsJoined))
            //.Sort("{MeaningsJoined.Translations[0]:1}")
            .Limit(limit)
            .ToListAsync();

        return result.With(new GetWordsQueryResponse(
            new WordsDTO(
                Words: words.Select(w => new WordDTO(
                    w.Id.Value, 
                    w.Value,
                    w.MeaningsJoined?.FirstOrDefault()?.Translations?.FirstOrDefault(),
                    w.PartsOfSpeech?.FirstOrDefault()?.Value,
                    w.MeaningsJoined?.FirstOrDefault()?.HiraganaWritings?.FirstOrDefault()?.Value)).ToArray(),
                Range: new RangeDTO((int) totalCount, range.Start, limit))));
    }

    public record WordProjection(WordId Id, string Value, List<PartOfSpeech> PartsOfSpeech, WordMeaningId[] Meanings);
    public record LookupResult(WordId Id, string Value, List<PartOfSpeech> PartsOfSpeech, WordMeaningId[] Meanings, WordMeaning[] MeaningsJoined);
}
