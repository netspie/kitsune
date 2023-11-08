using Corelibs.Basic.Blocks;
using Mediator;

namespace Manabu.UseCases.Content.Words;

public record GetWordQuery(
    string WordId) : IQuery<Result<GetWordQueryResponse>>;

public record GetWordQueryResponse(WordDetailsDTO Content);

public record WordDetailsDTO(
    string WordId,
    string Value,
    WordMeaningDTO[] Meanings,
    string[] PartOfSpeches,
    string[]? Properties = null,
    WordInflectionPairDTO[]? Inflections = null);

public record WordMeaningDTO(
    string Id,
    string Value);

public record WordInflectionPairDTO(
    string Name,
    WordInflectionFormDTO Informal,
    WordInflectionFormDTO? Formal = null);

public record WordInflectionFormDTO(
    string Positive,
    string? Negative = null);