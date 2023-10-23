using Corelibs.Basic.Blocks;
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
        var wm = await wordMeaningsCollection.Get<WordMeaning, WordMeaningId, WordMeaningProjection>(new WordMeaningId(query.WordMeaningId), b => b
            .Exclude(x => x.Id)
            .Include(x => x.WordId)
            .Include(x => x.Original)
            .Include(x => x.Translations)
            .Include(x => x.KanjiWritingPreferred));

        return result.With(new GetWordMeaningQueryResponse(
            new WordMeaningDetailsDTO(
                wm.WordId.Value,
                query.WordMeaningId,
                wm.Original,
                wm.Translations,
                null,
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
}
