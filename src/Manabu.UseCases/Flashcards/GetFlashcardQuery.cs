using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using Manabu.Entities.Flashcards;
using Mediator;

namespace Manabu.UseCases.Flashcards;

public class GetFlashcardQueryHandler : IQueryHandler<GetFlashcardQuery, Result<GetFlashcardQueryResponse>>
{
    private readonly IFlashcardResolver _flashcardResolver;

    public GetFlashcardQueryHandler(
        IFlashcardResolver flashcardResolver)
    {
        _flashcardResolver = flashcardResolver;
    }

    public async ValueTask<Result<GetFlashcardQueryResponse>> Handle(GetFlashcardQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetFlashcardQueryResponse>.Success();

        var flashcardResult = await _flashcardResolver.Get(card, query.Type, query.Mode);
        if (!flashcardResult.ValidateSuccessAndValues())
            return result.Fail();

        return result.With(new GetFlashcardQueryResponse(flashcardResult.Get()));
    }
}

public record GetFlashcardQuery(
    string ItemId,
    string Type,
    string Mode) : IQuery<Result<GetFlashcardQueryResponse>>;

public record GetFlashcardQueryResponse(FlashcardDTO Flashcard);

public record FlashcardDTO(
    string Id,
    FlashcardItemDTO[] Questions,
    FlashcardItemDTO[][] Answers,
    int[]? LevelIndexes = null);

public record FlashcardItemDTO(
    string Id,
    string Text,
    string Type);
