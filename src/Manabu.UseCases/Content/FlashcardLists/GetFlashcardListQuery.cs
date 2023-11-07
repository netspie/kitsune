using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Repository;
using Manabu.Entities.Content.Conversations;
using Manabu.Entities.Content.Courses;
using Manabu.Entities.Content.FlashcardLists;
using Manabu.Entities.Content.Lessons;
using Manabu.Entities.Content.Phrases;
using Manabu.Entities.Shared;
using Mediator;

namespace Manabu.UseCases.Content.FlashcardLists;

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

        var rootItemType = new LearningObjectType(query.RootItemType);
        var targetItemType = new LearningItemType(query.TargetItemType);
        var flashcardMode = new LearningMode(query.FlashcardMode);
        
        if (targetItemType == LearningItemType.Phrase)
        {
            var phrases = await GetPhrases(new LearningObjectId(query.RootItemId), rootItemType, _lessonRepository, _conversationRepository);
            return result.With(new GetFlashcardListQueryResponse(
                new FlashcardListDTO(query.RootItemType, query.FlashcardMode, phrases.Select(p => p.Value).ToArray())));
        }

        return result.Fail();
    }

    public static async Task<PhraseId[]> GetPhrases(
        LearningObjectId objectId,
        LearningObjectType itemType,
        IRepository<Lesson, LessonId> lessonRepository,
        IRepository<Conversation, ConversationId> conversationRepository)
    {
        var result = Result.Success();

        var phraseIds = new List<PhraseId>();
        if (itemType == LearningContainerType.Lesson.ToObjectType())
        {
            var lesson = await lessonRepository.Get(new LessonId(objectId.Value), result);
            var conversations = await conversationRepository.Get(lesson.Conversations ?? new(), result);
            
            var convPhraseIds = conversations.SelectMany(c => c.Phrases.SelectOrEmpty(p => p.Phrase)).ToArray();
            phraseIds.AddRange(convPhraseIds);
            phraseIds.AddRange(lesson.Phrases ?? new());
        }

        if (itemType == LearningContainerType.Conversation.ToObjectType())
        {
            var conversation = await conversationRepository.Get(new ConversationId(objectId.Value), result);

            var convPhraseIds = conversation.Phrases.SelectOrEmpty(x => x.Phrase).ToArray();
            phraseIds.AddRange(convPhraseIds);
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
