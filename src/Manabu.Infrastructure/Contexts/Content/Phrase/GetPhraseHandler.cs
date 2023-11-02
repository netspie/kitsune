using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Repository;
using Corelibs.MongoDB;
using Manabu.Entities.Content.Audios;
using Manabu.Entities.Content.Conversations;
using Manabu.Entities.Content.Courses;
using Manabu.Entities.Content.Lessons;
using Manabu.Entities.Content.Phrases;
using Manabu.Entities.Rehearse.RehearseEntities;
using Manabu.Entities.Shared;
using Manabu.UseCases.Content.Phrases;
using Mediator;
using MongoDB.Driver;

namespace Manabu.Infrastructure.Contexts.Content.Phrases;

public class GetPhraseHandler : IQueryHandler<GetPhraseQuery, Result<GetPhraseQueryResponse>>
{
    private readonly MongoConnection _mongoConnection;
    private readonly IRepository<Course, CourseId> _courseRepository;
    private readonly IRepository<Lesson, LessonId> _lessonRepository;
    private readonly IRepository<Conversation, ConversationId> _conversationRepository;
    private readonly IRepository<Phrase, PhraseId> _phraseRepository;
    private readonly IRepository<Audio, AudioId> _audioRepository;

    public GetPhraseHandler(
        MongoConnection mongoConnection,
        IRepository<Course, CourseId> courseRepository,
        IRepository<Lesson, LessonId> lessonRepository,
        IRepository<Conversation, ConversationId> conversationRepository,
        IRepository<Phrase, PhraseId> phraseRepository,
        IRepository<Audio, AudioId> audioRepository)
    {
        _mongoConnection = mongoConnection;
        _courseRepository = courseRepository;
        _lessonRepository = lessonRepository;
        _conversationRepository = conversationRepository;
        _phraseRepository = phraseRepository;
        _audioRepository = audioRepository;
    }

    public async ValueTask<Result<GetPhraseQueryResponse>> Handle(GetPhraseQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetPhraseQueryResponse>.Success();


        var phrase = await _phraseRepository.Get(new PhraseId(query.PhraseId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();
        var audios = await _audioRepository.Get(phrase.Audios ?? new(), result);

        var learningObjectId = new LearningObjectId(query.PhraseId);

        var collection = _mongoConnection.Database.GetCollection<RehearseEntity>(RehearseEntity.DefaultCollectionName);
        var rehearseEntityCount = collection.CountDocuments(Builders<RehearseEntity>.Filter.Eq("_id", learningObjectId));

        return result.With(
            new GetPhraseQueryResponse(
                new(phrase.Id.Value,
                    phrase.Original,
                    phrase.Translations.ToArrayOrEmpty(),
                    phrase.Contexts.ToArrayOrEmpty(),
                    Learned: rehearseEntityCount > 0,
                    audios.Select(a => new AudioDTO(a.Id.Value, a.Href)).ToArray())));
    }
}
