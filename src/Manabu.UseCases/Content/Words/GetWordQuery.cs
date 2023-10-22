using Corelibs.Basic.Blocks;
using Mediator;

namespace Manabu.UseCases.Content.Words;

public record GetWordQuery(
    string WordId) : IQuery<Result<GetWordQueryResponse>>;

public record GetWordQueryResponse(WordDetailsDTO Content);

public record WordDetailsDTO(
    string WordId,
    string Value,
    string[] Meanings,
    string[] PartOfSpeches);
