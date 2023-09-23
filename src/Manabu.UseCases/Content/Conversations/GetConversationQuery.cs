using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Repository;
using Manabu.Entities.Content.Conversations;
using Manabu.Entities.Content.Courses;
using Manabu.Entities.Content.Lessons;
using Manabu.Entities.Content.Phrases;
using Manabu.Entities.Shared;
using Mediator;

namespace Manabu.UseCases.Content.Conversations;

public class GetConversationQueryHandler : IQueryHandler<GetConversationQuery, Result<GetConversationQueryResponse>>
{
    private readonly IRepository<Course, CourseId> _courseRepository;
    private readonly IRepository<Lesson, LessonId> _lessonRepository;
    private readonly IRepository<Conversation, ConversationId> _conversationRepository;
    private readonly IRepository<Phrase, PhraseId> _phraseRepository;

    public GetConversationQueryHandler(
        IRepository<Course, CourseId> courseRepository,
        IRepository<Lesson, LessonId> lessonRepository,
        IRepository<Conversation, ConversationId> conversationRepository,
        IRepository<Phrase, PhraseId> phraseRepository)
    {
        _courseRepository = courseRepository;
        _lessonRepository = lessonRepository;
        _conversationRepository = conversationRepository;
        _phraseRepository = phraseRepository;
    }

    public async ValueTask<Result<GetConversationQueryResponse>> Handle(GetConversationQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetConversationQueryResponse>.Success();

        var conversation = await _conversationRepository.Get(new ConversationId(query.ConversationId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        var phrasesMode = new LearningPropertyType(query.PhrasesMode);

        var phrasesIds = conversation.Phrases.SelectOrDefault(p => p.Phrase).ToList();
        var phrases = await _phraseRepository.Get(phrasesIds, result);
        var lessons = await _lessonRepository.Get(conversation.Lessons ?? new(), result);

        return result.With(
            new GetConversationQueryResponse(
                new(conversation.Id.Value,
                    conversation.Name,
                    conversation.Description,
                    lessons
                        .OrderBy(p => conversation.Lessons.IndexOf(p.Id))
                        .Select((l, i) => new LessonDTO(l.Id.Value, l.Name))
                        .ToArray(),
                    phrases
                        .OrderBy(p => phrasesIds.IndexOf(p.Id))
                        .Select((p, i) =>
                        {
                            var phraseData = conversation.Phrases.FirstOrDefault(p2 => p2.Phrase == p.Id);
                            var phraseText = phrasesMode == LearningPropertyType.Translation ? p.Translations[0] : p.Original;
                            return new PhraseDTO(
                                p.Id.Value,
                                phraseText,
                                Speaker: phraseData?.Speaker,
                                SpeakerTranslation: phraseData?.SpeakerTranslation);
                         })
                        .ToArray())));
    }
}

public record GetConversationQuery(
    string ConversationId,
    string PhrasesMode = null) : IQuery<Result<GetConversationQueryResponse>>;

public record GetConversationQueryResponse(ConversationDetailsDTO Content);

public record ConversationDetailsDTO(
    string Id,
    string Name,
    string Description,
    LessonDTO[] Lessons,
    PhraseDTO[] Phrases);

public record LessonDTO(
    string Id,
    string Name);

public record PhraseDTO(
    string Id,
    string Original,
    string? Speaker = null,
    string? SpeakerTranslation = null);
