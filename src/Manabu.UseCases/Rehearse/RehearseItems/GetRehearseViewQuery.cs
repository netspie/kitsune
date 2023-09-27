using Corelibs.Basic.Blocks;
using Mediator;

namespace Manabu.UseCases.Rehearse.RehearseItems;

public record GetRehearseViewQuery() : IQuery<Result<GetRehearseViewQueryResponse>>;

public record GetRehearseViewQueryResponse(RehearseViewDTO Content);

public record RehearseViewDTO(
    int TotalRehearseItems,
    int TotalRehearseCollections);
