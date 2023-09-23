using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using Manabu.Entities.Flashcards;
using Manabu.Entities.Content.Audios;
using Manabu.Entities.Content.Phrases;
using Manabu.UseCases.Content.Flashcards;

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

        var type = new LearningItemType(itemTypeStr);
        var mode = new LearningMode(modeStr);

        if (type == LearningItemType.Phrase)
            await GetPhrase(itemId, result, mode);

        return result;
    }

    private async Task GetPhrase(string phraseId, Result<FlashcardDTO> result, LearningMode mode)
    {
        var phrase = await _phraseRepository.Get(new PhraseId(phraseId), result);
        if (mode == LearningMode.Reading)
        {
            // Questions
            var questions = new List<FlashcardItemDTO>();
            questions.Add(new(phrase.Id.Value, phrase.Original, LearningItemType.Phrase.Value));

            if (phrase.Contexts?.Count > 0)
                questions.Add(new FlashcardItemDTO(phraseId, phrase.Contexts.First(), LearningItemType.Context.Value));

            // Answers
            var answers = new List<FlashcardItemDTO>();
            answers.Add(new(phraseId, phrase.Translations.First(), LearningItemType.Phrase.Value));

            if (phrase.Audios?.Count > 0)
            {
                var audio = await _audioRepository.Get(phrase.Audios.First(), result);
                answers.Add(new FlashcardItemDTO(phraseId, audio.Href, LearningItemType.Audio.Value));
            }

            result.Add(new FlashcardDTO(
                phraseId, questions.ToArray(), new[] { answers.ToArray() }));
        }
        else
        if (mode == LearningMode.Listening)
        {
            // Questions
            var questions = new List<FlashcardItemDTO>();
            questions.Add(new(phrase.Id.Value, phrase.Audios.First().Value, LearningItemType.Audio.Value));

            if (phrase.Contexts?.Count > 0)
                questions.Add(new FlashcardItemDTO(phraseId, phrase.Contexts.First(), LearningItemType.Context.Value));

            // Answers
            var answers = new List<FlashcardItemDTO>();
            answers.Add(new(phrase.Id.Value, phrase.Original, LearningItemType.Phrase.Value));
            answers.Add(new(phraseId, phrase.Translations.First(), LearningItemType.Phrase.Value));

            result.Add(new FlashcardDTO(
                phraseId, questions.ToArray(), new[] { answers.ToArray() }));
        }
        else
        if (mode == LearningMode.Speaking)
        {
            // Questions
            var questions = new List<FlashcardItemDTO>();
            questions.Add(new(phraseId, phrase.Translations.First(), LearningItemType.Phrase.Value));

            if (phrase.Contexts?.Count > 0)
                questions.Add(new FlashcardItemDTO(phraseId, phrase.Contexts.First(), LearningItemType.Context.Value));

            // Answers
            var answers = new List<FlashcardItemDTO>();
            answers.Add(new(phrase.Id.Value, phrase.Original, LearningItemType.Phrase.Value));

            if (phrase.Audios?.Count > 0)
                answers.Add(new(phrase.Id.Value, phrase.Audios.First().Value, LearningItemType.Audio.Value));

            result.Add(new FlashcardDTO(
                phraseId, questions.ToArray(), new[] { answers.ToArray() }));
        }
    }
}
