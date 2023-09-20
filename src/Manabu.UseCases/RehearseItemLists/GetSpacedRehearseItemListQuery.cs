using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Repository;
using Manabu.Entities.Audios;
using Manabu.Entities.Conversations;
using Manabu.Entities.Courses;
using Manabu.Entities.Lessons;
using Manabu.Entities.Phrases;
using Manabu.Entities.RehearseItems;
using Manabu.Entities.RehearseSchedules;
using Manabu.Entities.Users;
using Mediator;

namespace Manabu.UseCases.RehearseItemLists;

public class GetRehearseItemListQueryHandler : IQueryHandler<GetSpacedRehearseItemListQuery, Result<GetRehearseItemListQueryResponse>>
{
    private readonly IRepository<User, UserId> _userRepository;
    private readonly IRepository<RehearseItem, RehearseItemId> _rehearseItemRepository;
    private readonly IRepository<RehearseSchedule, RehearseScheduleId> _rehearseScheduleRepository;
    private readonly IRepository<Course, CourseId> _courseRepository;
    private readonly IRepository<Lesson, LessonId> _lessonRepository;
    private readonly IRepository<Conversation, ConversationId> _conversationRepository;
    private readonly IRepository<Phrase, PhraseId> _phraseRepository;
    private readonly IRepository<Audio, AudioId> _audioRepository;

    public GetRehearseItemListQueryHandler(
        IRepository<User, UserId> userRepository,
        IRepository<RehearseItem, RehearseItemId> rehearseItemRepository,
        IRepository<RehearseSchedule, RehearseScheduleId> rehearseScheduleRepository,
        IRepository<Lesson, LessonId> lessonRepository,
        IRepository<Conversation, ConversationId> conversationRepository,
        IRepository<Phrase, PhraseId> phraseRepository,
        IRepository<Audio, AudioId> audioRepository)
    {
        _userRepository = userRepository;
        _rehearseItemRepository = rehearseItemRepository;
        _rehearseScheduleRepository = rehearseScheduleRepository;
        _lessonRepository = lessonRepository;
        _conversationRepository = conversationRepository;
        _phraseRepository = phraseRepository;
        _audioRepository = audioRepository;
    }

    public async ValueTask<Result<GetRehearseItemListQueryResponse>> Handle(GetSpacedRehearseItemListQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetRehearseItemListQueryResponse>.Success();

        var user = await _userRepository.Get(new UserId(query.UserId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();


        return result;
    }
}

public record GetSpacedRehearseItemListQuery(
    string UserId,
    string? Mode = null) : IQuery<Result<GetRehearseItemListQueryResponse>>;

public record GetRehearseItemListQueryResponse(RehearseItemListDTO Content);

public record RehearseItemListDTO(
    FlashcardListDTO[] Flashcards);

public record FlashcardListDTO(
    string Type,
    string Mode,
    string[] ItemIds);
