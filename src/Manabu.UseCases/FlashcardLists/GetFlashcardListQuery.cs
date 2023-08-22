using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using Manabu.Entities.FlashcardLists;
using Mediator;

namespace Manabu.UseCases.FlashcardLists;

public class GetFlashcardListQueryHandler : IQueryHandler<GetFlashcardListQuery, Result<GetFlashcardListQueryResponse>>
{
    private readonly IRepository<FlashcardList, FlashcardListId> _flashcardListRepository;

    public GetFlashcardListQueryHandler(IRepository<FlashcardList, FlashcardListId> flashcardListRepository)
    {
        _flashcardListRepository = flashcardListRepository;
    }

    public async ValueTask<Result<GetFlashcardListQueryResponse>> Handle(GetFlashcardListQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetFlashcardListQueryResponse>.Success();

        var list = await _flashcardListRepository.Get(new FlashcardListId(query.FlashcardListId), result);

        return result.With(new GetFlashcardListQueryResponse(
            new FlashcardListDTO(list.Type, list.Mode, list.Flashcards.Select(f => f.Value).ToArray())));
    }
}

public record GetFlashcardListQuery(
    string FlashcardListId) : IQuery<Result<GetFlashcardListQueryResponse>>;

public record GetFlashcardListQueryResponse(FlashcardListDTO FlashcardList);

public record FlashcardListDTO(
    string Type,
    string Mode,
    string[] FlashcardIds);
