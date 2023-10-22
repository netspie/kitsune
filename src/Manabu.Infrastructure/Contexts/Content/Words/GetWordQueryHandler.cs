using Corelibs.Basic.Blocks;
using Corelibs.MongoDB;
using Manabu.Entities.Content.WordMeanings;
using Manabu.Entities.Content.Words;
using Manabu.UseCases.Content.Words;
using Mediator;
using MongoDB.Driver;

namespace Manabu.Infrastructure.CQRS.Content.Lessons;

public class GetWordQueryHandler : IQueryHandler<GetWordQuery, Result<GetWordQueryResponse>>
{
    private readonly MongoConnection _mongoConnection;

    public GetWordQueryHandler(
        MongoConnection mongoConnection)
    {
        _mongoConnection = mongoConnection;
    }

    public async ValueTask<Result<GetWordQueryResponse>> Handle(GetWordQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetWordQueryResponse>.Success();

        var wordsCollection = _mongoConnection.Database.GetCollection<Word>(Word.DefaultCollectionName);
        var word = await wordsCollection.Get<Word, WordId, WordProjection>(new WordId(query.WordId), b => b
            .Exclude(x => x.Id)
            .Include(x => x.Value)
            .Include(x => x.PartsOfSpeech)
            .Include(x => x.Meanings)
            .Include(x => x.Properties));

        return result.With(new GetWordQueryResponse(
            new WordDetailsDTO(
                query.WordId,
                word.Value,
                word.Meanings.Select(m => m.Value).ToArray(),
                word.PartsOfSpeech.Select(m => m.Value).ToArray())));
    }

    public record WordProjection(
        string Value, 
        List<PartOfSpeech> PartsOfSpeech, 
        WordMeaningId[] Meanings,
        WordProperty[]? Properties = null);
}
