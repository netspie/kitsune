using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Functional;
using Corelibs.Basic.Repository;
using Manabu.Entities.Conversations;
using Manabu.Entities.Courses;
using Manabu.Entities.FlashcardLists;
using Manabu.Entities.Flashcards;
using Manabu.Entities.Lessons;
using Manabu.Entities.Phrases;
using Mediator;

namespace Manabu.UseCases.FlashcardLists;

public class GetFlashcardListQueryHandler : IQueryHandler<GetFlashcardListQuery, Result<GetFlashcardListQueryResponse>>
{
    private readonly IRepository<FlashcardList, FlashcardListId> _flashcardListRepository;

    private readonly IRepository<Course, CourseId> _courseRepository;
    private readonly IRepository<Lesson, LessonId> _lessonRepository;
    private readonly IRepository<Conversation, ConversationId> _conversationRepository;
    private readonly IRepository<Phrase, PhraseId> _phraseRepository;

    public GetFlashcardListQueryHandler(
        IRepository<FlashcardList, FlashcardListId> flashcardListRepository,
        IRepository<Lesson, LessonId> lessonRepository,
        IRepository<Course, CourseId> courseRepository,
        IRepository<Conversation, ConversationId> conversationRepository,
        IRepository<Phrase, PhraseId> phraseRepository)
    {
        _flashcardListRepository = flashcardListRepository;
        _lessonRepository = lessonRepository;
        _courseRepository = courseRepository;
        _conversationRepository = conversationRepository;
        _phraseRepository = phraseRepository;
    }

    public async ValueTask<Result<GetFlashcardListQueryResponse>> Handle(GetFlashcardListQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetFlashcardListQueryResponse>.Success();

        var rootItemType = new ItemType(query.RootItemType);
        var targetItemType = new ItemType(query.TargetItemType);
        var flashcardMode = new FlashcardMode(query.FlashcardMode);
        
        if (targetItemType == ItemType.Phrase)
        {
            var phrases = await GetPhrases(query.RootItemId, rootItemType);
            return result.With(new GetFlashcardListQueryResponse(
                new FlashcardListDTO(query.RootItemType, query.FlashcardMode, phrases.Select(p => p.Value).ToArray())));
        }

        return result.Fail();
    }

    private async Task<PhraseId[]> GetPhrases(string itemId, ItemType itemType)
    {
        var result = Result.Success();

        var phraseIds = new List<PhraseId>();
        if (itemType == ItemType.Lesson)
        {
            var lesson = await _lessonRepository.Get(new LessonId(itemId), result);
            var conversations = await _conversationRepository.Get(lesson.Conversations ?? new(), result);
            
            var convPhraseIds = conversations.SelectMany(c => c.Phrases.SelectOrDefault(p => p.Phrase)).ToArray();
            phraseIds.AddRange(convPhraseIds);
            phraseIds.AddRange(lesson.Phrases ?? new());
        }

        return phraseIds.ToArray();
    }
}

public record GetFlashcardListQuery(
    string RootItemId,
    string RootItemType,
    string TargetItemType,
    string FlashcardMode) : IQuery<Result<GetFlashcardListQueryResponse>>;

public record GetFlashcardListQueryResponse(FlashcardListDTO Content);

public record FlashcardListDTO(
    string Type,
    string Mode,
    string[] ItemIds);
