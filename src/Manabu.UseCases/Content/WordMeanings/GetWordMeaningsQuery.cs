using Corelibs.Basic.Blocks;
using Mediator;

namespace Manabu.UseCases.Content.WordMeanings;

public record GetWordMeaningsQuery(
    RangeArg? Range,
    ModifierArg[]? Modifiers = null) : IQuery<Result<GetWordMeaningsQueryResponse>>;

public record RangeArg(
    int Start,
    int Limit);

public record ModifierArg(
    string Field,
    bool IsSort = false,
    int Order = 1,
    string? Value = null);

public record GetWordMeaningsQueryResponse(WordMeaningsDTO Content);

public record WordMeaningsDTO(
    WordMeaningDTO[] Words,
    RangeDTO Range);

public record WordMeaningDTO(
    string Id,
    string WordId,
    string Word,
    string? Meaning,
    string? Reading);

public record RangeDTO(
    int TotalCount,
    int Start,
    int Limit);
