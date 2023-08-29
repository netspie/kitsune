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

        var phrase = await _phraseRepository.Get(new PhraseId(query.PhraseId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        return result.With(
            new GetPhraseQueryResponse(
                new(phrase.Id.Value,
                    phrase.Original,
                    phrase.Translations.ToArrayOrDefault(),
                    phrase.Contexts.ToArrayOrDefault(),
                    phrase.Audios.SelectOrDefault(a => a.Value).ToArray())));
    }
}

public record GetPhraseQuery(
    string PhraseId) : IQuery<Result<GetPhraseQueryResponse>>;

public record GetPhraseQueryResponse(PhraseDetailsDTO Content);

public record PhraseDetailsDTO(
    string Id,
    string Original,
    string[] Translations,
    string[] Contexts,
    string[] AudioIds,
    WordMeaningDTO[]? WordMeanings = null);

public record WordMeaningDTO(
    string Id,
    string Original,
    string WritingType);

public record WritingMode(string Value)
{
    public static readonly WritingMode Dictionary = new("dictionary");
    public static readonly WritingMode Hiragana = new("hiragana");
    public static readonly WritingMode Katakana = new("katakana");
    public static readonly WritingMode OtherWriting = new("otherWriting");
}
