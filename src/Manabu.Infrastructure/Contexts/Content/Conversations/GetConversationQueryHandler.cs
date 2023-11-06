using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Repository;
using Corelibs.MongoDB;
using Manabu.Entities.Content.Conversations;
using Manabu.Entities.Content.Courses;
using Manabu.Entities.Content.Lessons;
using Manabu.Entities.Content.Phrases;
using Manabu.Entities.Rehearse.RehearseEntities;
using Manabu.Entities.Shared;
using Manabu.UseCases.Content.Conversations;
using Mediator;
using MongoDB.Driver;

namespace Manabu.Infrastructure.Contexts.Content.Conversations;

public class GetConversationQueryHandler : IQueryHandler<GetConversationQuery, Result<GetConversationQueryResponse>>
{
    
    private readonly MongoConnection _mongoConnection;
    private readonly IRepository<Course, CourseId> _courseRepository;
    private readonly IRepository<Lesson, LessonId> _lessonRepository;
    private readonly IRepository<Conversation, ConversationId> _conversationRepository;
    private readonly IRepository<Phrase, PhraseId> _phraseRepository;

    public GetConversationQueryHandler(
        IRepository<Course, CourseId> courseRepository,
        IRepository<Lesson, LessonId> lessonRepository,
        IRepository<Conversation, ConversationId> conversationRepository,
        IRepository<Phrase, PhraseId> phraseRepository, MongoConnection mongoConnection)
    {
        _courseRepository = courseRepository;
        _lessonRepository = lessonRepository;
        _conversationRepository = conversationRepository;
        _phraseRepository = phraseRepository;
        _mongoConnection = mongoConnection;
    }

    public async ValueTask<Result<GetConversationQueryResponse>> Handle(GetConversationQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetConversationQueryResponse>.Success();

        var conversation = await _conversationRepository.Get(new ConversationId(query.ConversationId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        var phrasesMode = new LearningPropertyType(query.PhrasesMode);
        var learningObjectId = new LearningObjectId(query.ConversationId);
        
        var collection = _mongoConnection.Database.GetCollection<RehearseEntity>(RehearseEntity.DefaultCollectionName);
        var rehearseEntityCount = collection.CountDocuments(Builders<RehearseEntity>.Filter.Eq("_id", learningObjectId));
        
        var phrasesIds = conversation.Phrases.SelectOrEmpty(p => p.Phrase).ToList();
        var phrases = await _phraseRepository.Get(phrasesIds, result);
        var lessons = await _lessonRepository.Get(conversation.Lessons ?? new(), result);

        return result.With(
            new GetConversationQueryResponse(
                new(conversation.Id.Value,
                    conversation.Name,
                    conversation.Description,
                    Learned: rehearseEntityCount > 0,
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