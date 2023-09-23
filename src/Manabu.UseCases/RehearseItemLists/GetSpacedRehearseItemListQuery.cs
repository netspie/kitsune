using Corelibs.Basic.Auth;
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
using System.Security.Claims;

namespace Manabu.UseCases.RehearseItemLists;

//public class GetRehearseItemListQueryHandler : IQueryHandler<GetSpacedRehearseItemListQuery, Result<GetRehearseItemListQueryResponse>>
//{
//    private readonly IAccessorAsync<ClaimsPrincipal> _userAccessor;
//    private readonly IRepository<User, UserId> _userRepository;
//    private readonly IRepository<RehearseItem, RehearseItemId> _rehearseItemRepository;
//    private readonly IRepository<RehearseSchedule, RehearseScheduleId> _rehearseScheduleRepository;
//    private readonly IRepository<Course, CourseId> _courseRepository;
//    private readonly IRepository<Lesson, LessonId> _lessonRepository;
//    private readonly IRepository<Conversation, ConversationId> _conversationRepository;
//    private readonly IRepository<Phrase, PhraseId> _phraseRepository;
//    private readonly IRepository<Audio, AudioId> _audioRepository;

//    public GetRehearseItemListQueryHandler(
//        IRepository<User, UserId> userRepository,
//        IRepository<RehearseItem, RehearseItemId> rehearseItemRepository,
//        IRepository<RehearseSchedule, RehearseScheduleId> rehearseScheduleRepository,
//        IRepository<Lesson, LessonId> lessonRepository,
//        IRepository<Conversation, ConversationId> conversationRepository,
//        IRepository<Phrase, PhraseId> phraseRepository,
//        IRepository<Audio, AudioId> audioRepository,
//        IAccessorAsync<ClaimsPrincipal> userAccessor)
//    {
//        _userRepository = userRepository;
//        _rehearseItemRepository = rehearseItemRepository;
//        _rehearseScheduleRepository = rehearseScheduleRepository;
//        _lessonRepository = lessonRepository;
//        _conversationRepository = conversationRepository;
//        _phraseRepository = phraseRepository;
//        _audioRepository = audioRepository;
//        _userAccessor = userAccessor;
//    }

//    public async ValueTask<Result<GetRehearseItemListQueryResponse>> Handle(GetSpacedRehearseItemListQuery query, CancellationToken cancellationToken)
//    {
//        var result = Result<GetRehearseItemListQueryResponse>.Success();

//        var userId = await _userAccessor.GetUserID<UserId>();

//        var user = await _userRepository.Get(userId, result);
//        if (!result.ValidateSuccessAndValues())
//            return result.Fail();



//        return result;
//    }
//}

public record GetSpacedRehearseItemListQuery(
    string? Mode = null,
    string? ItemType = null) : IQuery<Result<GetRehearseItemListQueryResponse>>;

public record GetRehearseItemListQueryResponse(RehearseItemListDTO Content);

public record RehearseItemListDTO(
    FlashcardListDTO[] Flashcards);

public record FlashcardListDTO(
    string Type,
    string Mode,
    string[] ItemIds);
