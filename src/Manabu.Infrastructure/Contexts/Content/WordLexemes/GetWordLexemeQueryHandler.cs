using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.MongoDB;
using Manabu.Entities.Content.WordLexemes;
using Manabu.Entities.Content.Words;
using Manabu.UseCases.Content.WordLexemes;
using Mediator;
using MongoDB.Driver;

namespace Manabu.Infrastructure.Contexts.Content.WordLexemes;

public class GetWordLexemeQueryHandler : IQueryHandler<GetWordLexemeQuery, Result<GetWordLexemeQueryResponse>>
{
    private readonly MongoConnection _mongoConnection;

    public GetWordLexemeQueryHandler(
        MongoConnection mongoConnection)
    {
        _mongoConnection = mongoConnection;
    }

    public async ValueTask<Result<GetWordLexemeQueryResponse>> Handle(GetWordLexemeQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetWordLexemeQueryResponse>.Success();

        var wordCollection = _mongoConnection.Database.GetCollection<Word>(Word.DefaultCollectionName);
        var wordQueryable = wordCollection.AsQueryable();

        var word = wordQueryable.FirstOrDefault(x => x.Id.Value == query.WordId);
        if (word?.Lexeme is null)
            return result.Fail();

        var lexemeCollection = _mongoConnection.Database.GetCollection<WordLexeme>(WordLexeme.DefaultCollectionName);
        var lexemeQueryable = lexemeCollection.AsQueryable();

        var lexeme = lexemeQueryable.FirstOrDefault(x => x.Id == word.Lexeme);
        if (lexeme is null)
            return result.Fail();

        return result.With(
            new GetWordLexemeQueryResponse(
                new(lexeme.Id.Value,
                    word.Id.Value,
                    lexeme.Inflections.Select(pair => 
                        new WordInflectionPairDTO(
                            pair.Type.Value,
                            new WordInflectionFormDTO(pair.Informal.Positive.Value, pair.Informal?.Negative?.Value),
                            pair.Formal is null ? null : new WordInflectionFormDTO(pair.Formal.Positive.Value, pair?.Formal?.Negative?.Value))).ToArray())));
    }
}
