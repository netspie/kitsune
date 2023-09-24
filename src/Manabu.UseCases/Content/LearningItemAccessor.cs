using Corelibs.Basic.Blocks;
using Corelibs.Basic.DDD;
using Corelibs.Basic.Repository;
using Manabu.Entities.Content.Lessons;
using Manabu.Entities.Content.Phrases;
using Manabu.Entities.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Manabu.UseCases.Content;

public class LearningItemAccessor
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public LearningItemAccessor(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task GetNestedLearningObjects(
        LearningObjectId learningObjectId, 
        LearningObjectType learningObjectType, 
        List<EntityId> allLearningObjectsFound,
        Result result)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var sp = scope.ServiceProvider;

        return GetNestedLearningObjectsImpl(
            sp,
            learningObjectId,
            learningObjectType,
            allLearningObjectsFound,
            result);
    }

    private async Task GetNestedLearningObjectsImpl(
        IServiceProvider sp,
        LearningObjectId learningObjectId,
        LearningObjectType learningObjectType,
        List<EntityId> allLearningObjectsFound,
        Result result)
    {
        //_serviceProvider.GetService

        if (learningObjectType == LearningContainerType.Lesson)
        {
            var lessonRepo = sp.GetRequiredService<IRepository<Lesson, LessonId>>();

            var lessonId = new LessonId(learningObjectId.Value);
            var lesson = await lessonRepo.Get(lessonId, result);

            foreach (var phraseId in lesson.Phrases)
            {
                var phraseRepo = sp.GetRequiredService<IRepository<Phrase, PhraseId>>();
                var phrase = await phraseRepo.Get(phraseId, result);

            }
            //allLearningObjectsFound.Add(lessonId)
        }
    }

    private async Task GetNestedLearningObjectsImpl(
        IServiceProvider sp,
        IEntity learningObject,
        List<EntityId> allLearningObjectsFound,
        Result result)
    {
        if (learningObject is Lesson lesson)
        {
            await GetPhrasesAndNestedObjects(sp, lesson.Phrases, allLearningObjectsFound, result);
            //allLearningObjectsFound.Add(lessonId)
        }
    }

    private async Task GetPhrasesAndNestedObjects(
        IServiceProvider sp,
        IEnumerable<PhraseId> phraseIds,
        List<EntityId> allLearningObjectsFound,
        Result result)
    {
        foreach (var phraseId in phraseIds)
        {
            var phraseRepository = sp.GetRequiredService<IRepository<Phrase, PhraseId>>();
            var phrase = await phraseRepository.Get(phraseId, result);

            allLearningObjectsFound.Add(phraseId);

            await GetNestedLearningObjectsImpl(
                sp, phrase, allLearningObjectsFound, result);
        }
    }
}
