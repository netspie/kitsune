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

public record GetWordMeaningsQueryResponse(WordsDTO Content);

public record WordsDTO(
    WordDTO[] Words,
    RangeDTO Range);

public record WordDTO(
    string Id,
    string Value,
    string? Meaning,
    string? PartOfSpeech,
    string? Reading);

public record RangeDTO(
    int TotalCount,
    int Start,
    int Limit);
