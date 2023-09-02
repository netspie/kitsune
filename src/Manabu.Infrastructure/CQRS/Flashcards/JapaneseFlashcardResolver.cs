using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using Manabu.Entities.Audios;
using Manabu.Entities.Flashcards;
using Manabu.Entities.Phrases;
using Manabu.UseCases.Flashcards;

namespace Manabu.Infrastructure.CQRS.Flashcards;

public class JapaneseFlashcardResolver : IFlashcardResolver
{
    private readonly IRepository<Phrase, PhraseId> _phraseRepository;
    private readonly IRepository<Audio, AudioId> _audioRepository;

    public JapaneseFlashcardResolver(
        IRepository<Phrase, PhraseId> phraseRepository, 
        IRepository<Audio, AudioId> audioRepository)
    {
        _phraseRepository = phraseRepository;
        _audioRepository = audioRepository;
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
                questions.Add(new FlashcardItemDTO(phraseId, phrase.Contexts.First(), ItemType.Context.Value));

            // Answers
            var answers = new List<FlashcardItemDTO>();
            answers.Add(new(phraseId, phrase.Translations.First(), ItemType.Phrase.Value));

            if (phrase.Audios?.Count > 0)
            {
                var audio = await _audioRepository.Get(phrase.Audios.First(), result);
                answers.Add(new FlashcardItemDTO(phraseId, audio.Href, ItemType.Audio.Value));
            }

            result.Add(new FlashcardDTO(
                phraseId, questions.ToArray(), new[] { answers.ToArray() }));
        }
        else
        if (mode == FlashcardMode.Listening)
        {
            // Questions
            var questions = new List<FlashcardItemDTO>();
            questions.Add(new(phrase.Id.Value, phrase.Audios.First().Value, ItemType.Audio.Value));

            if (phrase.Contexts?.Count > 0)
                questions.Add(new FlashcardItemDTO(phraseId, phrase.Contexts.First(), ItemType.Context.Value));

            // Answers
            var answers = new List<FlashcardItemDTO>();
            answers.Add(new(phrase.Id.Value, phrase.Original, ItemType.Phrase.Value));
            answers.Add(new(phraseId, phrase.Translations.First(), ItemType.Phrase.Value));

            result.Add(new FlashcardDTO(
                phraseId, questions.ToArray(), new[] { answers.ToArray() }));
        }
        else
        if (mode == FlashcardMode.Speaking)
        {
            // Questions
            var questions = new List<FlashcardItemDTO>();
            questions.Add(new(phraseId, phrase.Translations.First(), ItemType.Phrase.Value));

            if (phrase.Contexts?.Count > 0)
                questions.Add(new FlashcardItemDTO(phraseId, phrase.Contexts.First(), ItemType.Context.Value));

            // Answers
            var answers = new List<FlashcardItemDTO>();
            answers.Add(new(phrase.Id.Value, phrase.Original, ItemType.Phrase.Value));

            if (phrase.Audios?.Count > 0)
                answers.Add(new(phrase.Id.Value, phrase.Audios.First().Value, ItemType.Audio.Value));

            result.Add(new FlashcardDTO(
                phraseId, questions.ToArray(), new[] { answers.ToArray() }));
        }
    }
}
