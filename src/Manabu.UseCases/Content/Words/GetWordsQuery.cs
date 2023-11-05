using Corelibs.Basic.Blocks;
using Mediator;

namespace Manabu.UseCases.Content.Words;

public record GetWordsQuery(
    RangeArg? Range,
    SortArg[]? SortArgs = null,
    FilterArg[]? FilterArgs = null) : IQuery<Result<GetWordsQueryResponse>>;

public record RangeArg(
    int Start,
    int Limit);

public record SortArg(
    string Field,
    int Order = 1);

public record FilterArg(
    string Field,
    string Value);

public record GetWordsQueryResponse(WordsDTO Content);

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
