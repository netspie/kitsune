using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Repository;
using Manabu.Entities.Conversations;
using Manabu.Entities.Courses;
using Manabu.Entities.Lessons;
using Manabu.Entities.Phrases;
using Mediator;

namespace Manabu.UseCases.Phrases;

public class GetPhraseQueryHandler : IQueryHandler<GetPhraseQuery, Result<GetPhraseQueryResponse>>
{
    private readonly IRepository<Course, CourseId> _courseRepository;
    private readonly IRepository<Lesson, LessonId> _lessonRepository;
    private readonly IRepository<Conversation, ConversationId> _conversationRepository;
    private readonly IRepository<Phrase, PhraseId> _phraseRepository;

    public GetPhraseQueryHandler(
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

    public async ValueTask<Result<GetPhraseQueryResponse>> Handle(GetPhraseQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetPhraseQueryResponse>.Success();

        //var conversation = await _conversationRepository.Get(new ConversationId(query.ConversationId), result);
        //if (!result.ValidateSuccessAndValues())
        //    return result.Fail();

        //var phrasesIds = conversation.Phrases.SelectOrDefault(p => p.Phrase).ToList();
        //var phrases = await _phraseRepository.Get(phrasesIds, result);
        //var lessons = await _lessonRepository.Get(conversation.Lessons ?? new(), result);

        //return result.With(
        //    new GetPhraseQueryResponse(
        //        new(conversation.Id.Value,
        //            conversation.Name,
        //            conversation.Description,
        //            lessons
        //                .OrderBy(p => conversation.Lessons.IndexOf(p.Id))
        //                .Select((l, i) => new LessonDTO(l.Id.Value, l.Name))
        //                .ToArray(),
        //            phrases
        //                .OrderBy(p => phrasesIds.IndexOf(p.Id))
        //                .Select((p, i) => new PhraseDTO(p.Id.Value, p.Original, Speaker: conversation.Phrases.FirstOrDefault(p2 => p2.Phrase == p.Id)?.Speaker))
        //                .ToArray())));

        return result.Fail();
    }
}

public record GetPhraseQuery(
    string PhraseId) : IQuery<Result<GetPhraseQueryResponse>>;

public record GetPhraseQueryResponse(ConversationDetailsDTO Content);

public record ConversationDetailsDTO(
    string Id,
    string Original,
    string[] Translations,
    string[] Contexts,
    string[] AudioIds,
    WordMeaningDTO[] WordMeanings,
    PhraseDTO[] Phrases);

public record WordMeaningDTO(
    string Id,
    string Original,
    string WritingType);

public record PhraseDTO(
    string Id,
    string Original,
    string? Speaker = null);

public record WritingMode(string Value)
{
    public static readonly WritingMode Dictionary = new("dictionary");
    public static readonly WritingMode Hiragana = new("hiragana");
    public static readonly WritingMode Katakana = new("katakana");
    public static readonly WritingMode OtherWriting = new("inflection");
}
