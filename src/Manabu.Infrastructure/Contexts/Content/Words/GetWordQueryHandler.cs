using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.MongoDB;
using Manabu.Entities.Content.WordLexemes;
using Manabu.Entities.Content.WordMeanings;
using Manabu.Entities.Content.Words;
using Manabu.UseCases.Content.Words;
using Mediator;
using MongoDB.Driver;

namespace Manabu.Infrastructure.CQRS.Content.Words;

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
        var wordFilter = Builders<Word>.Filter.Eq(x => x.Id, new WordId(query.WordId));
        var wordProjection = Builders<Word>.Projection
            .Exclude(x => x.Id)
            .Include(x => x.Value)
            .Include(x => x.PartsOfSpeech)
            .Include(x => x.Meanings)
            .Include(x => x.Properties)
            .Include(x => x.Lexeme);

        var word = await wordsCollection
            .Aggregate()
            .Match(wordFilter)
            .Project(wordProjection)
            .Lookup<WordMeaning, LookupResult>(
                foreignCollectionName: WordMeaning.DefaultCollectionName,
                localField: nameof(Word.Meanings),
                foreignField: "_id",
                @as: nameof(LookupResult.MeaningsJoined))
            .Lookup<WordLexeme, LookupResult>(
                foreignCollectionName: WordLexeme.DefaultCollectionName,
                localField: nameof(Word.Lexeme),
                foreignField: "_id",
                @as: nameof(LookupResult.LexemeJoined))
            .FirstOrDefaultAsync();

        return result.With(new GetWordQueryResponse(
            new WordDetailsDTO(
                query.WordId,
                word.Value,
                word.MeaningsJoined.SelectOrEmpty(m => 
                    new WordMeaningDTO(m.Id.Value, m.Translations.AggregateOrDefault((x, y) => $"{x}, {y}"))).ToArray(),
                word.PartsOfSpeech.SelectOrEmpty(m => m.Value).ToArray(),
                word.Properties.SelectOrEmpty(m => m.Value).ToArray(),
                word.LexemeJoined.FirstOrDefault()?.Inflections.SelectOrEmpty(i => 
                    new WordInflectionPairDTO(i.Name, 
                        new WordInflectionFormDTO(i.Informal.Positive.Value, i.Informal.Negative?.Value),
                        i.Formal is null ? null : new WordInflectionFormDTO(i.Formal.Positive.Value, i.Formal.Negative?.Value))).ToArray())));
    }

    public record WordProjection(
        string Value, 
        List<PartOfSpeech> PartsOfSpeech,
        WordMeaningId[] Meanings,
        WordProperty[]? Properties,
        WordLexemeId? Lexeme);

    public record LookupResult(
        WordId Id, 
        string Value, 
        List<PartOfSpeech> PartsOfSpeech, 
        WordMeaningId[] Meanings,
        WordProperty[] Properties,
        WordLexemeId? Lexeme,
        WordMeaning[] MeaningsJoined,
        WordLexeme[]? LexemeJoined = null);

}
