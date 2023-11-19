using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.MongoDB;
using Manabu.Entities.Content.WordMeanings;
using Manabu.Entities.Content.Words;
using Manabu.UseCases.Content.WordMeanings;
using Mediator;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Search;

namespace Manabu.Infrastructure.CQRS.Content.WordMeanings;

using SearchDef = (string IndexName, SearchDefinition<WordMeaning> Definition);

public class GetWordMeaningsQueryHandler : IQueryHandler<GetWordMeaningsQuery, Result<GetWordMeaningsQueryResponse>>
{
    private const int MaxItemLimit = 20;

    private readonly MongoConnection _mongoConnection;

    public GetWordMeaningsQueryHandler(
        MongoConnection mongoConnection)
    {
        _mongoConnection = mongoConnection;
    }

    public async ValueTask<Result<GetWordMeaningsQueryResponse>> Handle(GetWordMeaningsQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetWordMeaningsQueryResponse>.Success();

        var wordsCollection = _mongoConnection.Database.GetCollection<WordMeaning>(WordMeaning.DefaultCollectionName);

        var range = query.Range ?? new RangeArg(0, MaxItemLimit);
        var limit = Math.Min(range.Limit, MaxItemLimit);

        var filter = Builders<WordMeaning>.Filter.Empty;

        List<SearchDef> searches = [];
        List<string> sorts = [];
        foreach (var arg in query.Modifiers.ToArrayOrEmpty())
        {
            if (arg.IsSort)
            {
                if (arg.Field == nameof(Word))
                    sorts.Add($"{{{nameof(WordMeaning.Original)}:{arg.Order}}}");

                if (arg.Field == "Meaning")
                    sorts.Add($"{{{nameof(WordMeaning.Translations)}:{arg.Order}}}");

                //if (arg.Field == "Part Of Speech")
                ///sorts.Add($"{{PartsOfSpeech:{arg.Order}}}");
            }
            else
            {
                if (arg.Field.IsNullOrEmpty() ||
                    arg.Value.IsNullOrEmpty())
                    continue;

                //if (arg.Field == "Part Of Speech");
                //filter &= Builders<WordMeaning>.Filter.Eq(x => x.PartsOfSpeech[0].Value, arg.Value.ToLower());
                //else
                if (arg.Field == nameof(Word) && MongoConfig.IsAtlas)
                    searches.Add(new("searchWordMeanings", Builders<WordMeaning>.Search.Regex(x => x.Original, $"(.*){arg.Value}(.*)")));
                else
                if (arg.Field == "Meaning" && MongoConfig.IsAtlas)
                    searches.Add(new("searchWordMeanings", Builders<WordMeaning>.Search.Regex(x => x.Translations, $"(.*){arg.Value}(.*)")));
            }
        }

        var wordMeaningProjection = Builders<WordMeaning>.Projection
            .Include(x => x.Id)
            .Include(x => x.Original)
            .Include(x => x.WordId)
            .Include(x => x.HiraganaWritings)
            .Include(x => x.Translations);

        var totalCount = await wordsCollection.CountDocumentsAsync(filter);

        var aggregate = wordsCollection.Aggregate(new AggregateOptions());
        foreach (var search in searches)
            aggregate = aggregate.Search(search.Definition, new SearchOptions<WordMeaning>() { IndexName = search.IndexName });

        aggregate = aggregate.Match(filter);
        foreach (var sort in sorts)
            aggregate = aggregate.Sort(sort);

        var wordMeanings = await aggregate
            .Project(wordMeaningProjection)
            .Skip(range.Start)
            .Limit(limit)
            .ToListAsync();

        return result.With(new GetWordMeaningsQueryResponse(
            new WordMeaningsDTO(
                Words: wordMeanings.Select(w =>
                {
                    var wm = BsonSerializer.Deserialize<WordMeaningProjection>(w);
                    return new WordMeaningDTO(
                        wm.Id.Value,
                        wm.Original,
                        wm.Translations?.FirstOrDefault(),
                        wm.HiraganaWritings?.FirstOrDefault()?.Value);
                }).ToArray(),
                Range: new RangeDTO((int) totalCount, range.Start, limit))));
    }

    public record WordMeaningProjection(WordMeaningId Id, WordId WordId, string Original, WordMeaning.HiraganaWriting[] HiraganaWritings, string[] Translations);
    //public record LookupResult(WordId Id, string Value, List<PartOfSpeech> PartsOfSpeech, WordMeaningId[] Meanings, WordMeaning[] MeaningsJoined);
}
