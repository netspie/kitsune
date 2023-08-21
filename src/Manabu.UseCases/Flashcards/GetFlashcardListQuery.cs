using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using Manabu.Entities.Flashcards;
using Mediator;

namespace Manabu.UseCases.Flashcards;

public class GetFlashcardListQueryHandler : IQueryHandler<GetFlashcardListQuery, Result<GetFlashcardListQueryResponse>>
{
    private readonly IRepository<FlashcardList, FlashcardListId> _flashcardListRepository;

    public async ValueTask<Result<GetFlashcardListQueryResponse>> Handle(GetFlashcardListQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetFlashcardListQueryResponse>.Success();

        var list = await _flashcardListRepository.Get(new FlashcardListId(query.FlashcardListId), result);

        if (list.Type == "phrase")
        {

        }
        else
        if (list.Type == "word")
        {

        }

        return result;
    }
}

public record GetFlashcardListQuery(
    string FlashcardListId) : IQuery<Result<GetFlashcardListQueryResponse>>;

public record GetFlashcardListQueryResponse();

public record FlashcardListDTO(
    string Type,
    string Mode, 
    string[] FlashcardIds);