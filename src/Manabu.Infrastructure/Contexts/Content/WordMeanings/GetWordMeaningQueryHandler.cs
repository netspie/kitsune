using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.MongoDB;
using Manabu.Entities.Content.WordMeanings;
using Manabu.Entities.Content.Words;
using Manabu.UseCases.Content.WordMeanings;
using Mediator;
using MongoDB.Driver;

namespace Manabu.Infrastructure.CQRS.Content.WordMeanings;

public class GetWordMeaningQueryHandler : IQueryHandler<GetWordMeaningQuery, Result<GetWordMeaningQueryResponse>>
{
    private readonly MongoConnection _mongoConnection;

    public GetWordMeaningQueryHandler(
        MongoConnection mongoConnection)
    {
        _mongoConnection = mongoConnection;
    }

    public async ValueTask<Result<GetWordMeaningQueryResponse>> Handle(GetWordMeaningQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetWordMeaningQueryResponse>.Success();

        var wordMeaningsCollection = _mongoConnection.Database.GetCollection<WordMeaning>(WordMeaning.DefaultCollectionName);

        var wordMeaningFilter = Builders<WordMeaning>.Filter.Eq(x => x.Id, new WordMeaningId(query.WordMeaningId));
        var wordMeaningProjection = Builders<WordMeaning>.Projection
            .Exclude(x => x.Id)
            .Include(x => x.WordId)
            .Include(x => x.Original)
            .Include(x => x.Translations)
            .Include(x => x.KanjiWritingPreferred);

        var wm = await wordMeaningsCollection
            .Aggregate()
            .Match(wordMeaningFilter)
            .Project(wordMeaningProjection)
            .Lookup<Word, LookupResult>(
                foreignCollectionName: Word.DefaultCollectionName,
                localField: nameof(WordMeaning.WordId),
                foreignField: "_id",
                @as: nameof(LookupResult.Words))
            .FirstOrDefaultAsync();

        return result.With(new GetWordMeaningQueryResponse(
            new WordMeaningDetailsDTO(
                wm.WordId.Value,
                query.WordMeaningId,
                wm.Original,
                wm.Translations,
                wm.Words[0].PartsOfSpeech
                    .Select(p => p.Value)
                    .Concat(wm.Words[0].Properties.SelectOrDefault(p => p.Value)).ToArray(),
                wm.PitchAccent,
                wm.KanjiWritingPreferred.HasValue ? wm.KanjiWritingPreferred.Value : true)));
    }

    public record WordMeaningProjection(
        WordId WordId,
        string Original,
        string[] Translations,
        string PitchAccent,
        bool? KanjiWritingPreferred);
    //List<PartOfSpeech> PartsOfSpeech);

    public record LookupResult(
        WordId WordId,
        string Original,
        string[] Translations,
        string PitchAccent,
        bool? KanjiWritingPreferred,
        Word[] Words);
}
