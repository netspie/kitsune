using Corelibs.Basic.Blocks;
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

        var flashcardResult = await _flashcardResolver.Get(query.TargetItemId, query.TargetItemType, query.FlashcardMode);
        if (!flashcardResult.ValidateSuccessAndValues())
            return result.Fail();

        return result.With(new GetFlashcardQueryResponse(flashcardResult.Get()));
    }
}

public record GetFlashcardQuery(
    string TargetItemId,
    string TargetItemType,
    string FlashcardMode) : IQuery<Result<GetFlashcardQueryResponse>>;

public record GetFlashcardQueryResponse(FlashcardDTO Flashcard);

public record FlashcardDTO(
    string ItemId,
    FlashcardItemDTO[] Questions,
    FlashcardItemDTO[][] Answers,
    int[]? LevelIndexes = null);

public record FlashcardItemDTO(
    string Id,
    string Text,
    string Type);
