using Corelibs.Basic.Blocks;

namespace Manabu.UseCases.Content.Flashcards;

public interface IFlashcardResolver
{
    Task<Result<FlashcardDTO>> Get(string itemId, string itemType, string mode);
}
