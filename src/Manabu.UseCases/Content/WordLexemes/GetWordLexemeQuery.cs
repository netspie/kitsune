using Corelibs.Basic.Blocks;
using Mediator;

namespace Manabu.UseCases.Content.WordLexemes;

public record GetWordLexemeQuery(
    string WordId) : IQuery<Result<GetWordLexemeQueryResponse>>;

public record GetWordLexemeQueryResponse(WordLexemeDTO Content);

public record WordLexemeDTO(
    string LexemeId,
    string WordId,
    WordInflectionPairDTO[] Inflections);

public record WordInflectionPairDTO(
    string Type,
    WordInflectionFormDTO Informal,
    WordInflectionFormDTO? Formal = null);

public record WordInflectionFormDTO(
    string Positive,
    string? Negative = null);
