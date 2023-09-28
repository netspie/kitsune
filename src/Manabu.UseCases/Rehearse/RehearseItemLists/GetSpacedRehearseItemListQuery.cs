using Corelibs.Basic.Blocks;
using Mediator;

namespace Manabu.UseCases.Rehearse.RehearseItemLists;

public record GetSpacedRehearseItemListQuery(
    string[]? Modes = null,
    string[]? ItemTypes = null,
    int[]? DayIntervals = null,
    bool Random = true) : IQuery<Result<GetSpacedRehearseItemListQueryResponse>>;

public record GetSpacedRehearseItemListQueryResponse(FlashcardListDTO Content);

public record FlashcardListDTO(
    FlashcardDTO[] Flashcards);

public record FlashcardDTO(
    string RehearseItemId,
    string LearningItemId,
    string Type,
    string Mode);
