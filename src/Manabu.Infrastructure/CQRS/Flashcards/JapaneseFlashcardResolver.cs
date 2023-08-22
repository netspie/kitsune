using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using Manabu.Entities.Courses;
using Manabu.Entities.Flashcards;
using Manabu.UseCases.Flashcards;

namespace Manabu.Infrastructure.CQRS.Flashcards;

public class JapaneseFlashcardResolver : IFlashcardResolver
{
    private readonly IRepository<Phrase, PhraseId> _phraseRepository;

    public JapaneseFlashcardResolver(IRepository<Phrase, PhraseId> phraseRepository)
    {
        _phraseRepository = phraseRepository;
    }

    public async Task<Result<FlashcardDTO>> Get(string itemId, string itemTypeStr, string modeStr)
    {
        var result = Result<FlashcardDTO>.Success();

        var type = new ItemType(itemTypeStr);
        var mode = new FlashcardMode(modeStr);

        if (type == ItemType.Phrase)
            await GetPhrase(itemId, result, mode);

        return result;
    }

    private async Task GetPhrase(string phraseId, Result<FlashcardDTO> result, FlashcardMode mode)
    {
        var phrase = await _phraseRepository.Get(new PhraseId(phraseId), result);
        if (mode == FlashcardMode.Reading)
        {
            // Questions
            var questions = new List<FlashcardItemDTO>();
            questions.Add(new(phrase.Id.Value, phrase.Original, ItemType.Phrase.Value));

            if (phrase.Contexts?.Count > 0)
                questions.Add(new FlashcardItemDTO(phrase.Id.Value, phrase.Contexts.First(), ItemType.Context.Value));

            // Answers
            var answers = new List<FlashcardItemDTO>();
            questions.Add(new(phrase.Id.Value, phrase.Translations.First(), ItemType.Phrase.Value));

            if (phrase.Audios?.Count > 0)
                questions.Add(new FlashcardItemDTO(phrase.Id.Value, phrase.Audios.First().Value, ItemType.Audio.Value));

            //result.Add(new FlashcardDTO(
            //    flashcard.Id.Value, questions.ToArray(), new[] { answers.ToArray() }, flashcard.LevelIndexes.ToArray()));
        }
    }
}
