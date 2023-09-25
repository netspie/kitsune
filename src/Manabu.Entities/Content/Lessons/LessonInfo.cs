using Corelibs.Basic.Collections;
using Corelibs.Basic.DDD;
using Manabu.Entities.Content._Shared;
using Manabu.Entities.Shared;

namespace Manabu.Entities.Content.Lessons;

public class LessonInfo : IProcessorEntityInfo<Lesson, LessonId>
{
    public bool IsLearningItem => false;
    public LearningMode[] LearningModes => Array.Empty<LearningMode>();
    public LearningObjectType LearningObjectType => LearningContainerType.Lesson.ToObjectType();
    public LessonId CreateId(LearningObjectId id) =>
        new LessonId(id.Value);

    public EntityId[] GetChildLearningItemIds(Lesson entity) =>
        ArrayExtensions.CreateArray<EntityId>(
            entity.Phrases,
            entity.Conversations,
            entity.Infos);

    public Type EntityType => typeof(Lesson);
    public Type IdType => typeof(LessonId);
}
