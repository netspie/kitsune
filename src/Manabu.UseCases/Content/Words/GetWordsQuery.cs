using Corelibs.Basic.Blocks;
using Mediator;

namespace Manabu.UseCases.Content.Words;

public record GetWordsQuery(
    int Limit = 10,
    RangeDTO? Range = null,
    SortArg[]? SortArgs = null,
    FilterArg[]? FilterArgs = null) : IQuery<Result<GetWordsQueryResponse>>;

public record SortArg(
    string Field,
    int Order = 1);

public record FilterArg(
    string Field,
    string Value);

public record GetWordsQueryResponse(WordsDTO Content);

public record WordsDTO(
    WordDTO[] Words,
    RangeDTO Range,
    bool IsThereMoreWords);

public record WordDTO(
    string Id,
    string Value,
    string Meaning,
    string PartOfSpeech);

public record RangeDTO(
    int Start,
    int? End = null);
