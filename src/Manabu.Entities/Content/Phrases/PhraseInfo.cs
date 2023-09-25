using Corelibs.Basic.Collections;
using Corelibs.Basic.DDD;
using Manabu.Entities.Content._Shared;
using Manabu.Entities.Shared;

namespace Manabu.Entities.Content.Phrases;

public class PhraseInfo : IProcessorEntityInfo<Phrase, PhraseId>
{
    public bool IsLearningItem => true;
    public LearningMode[] LearningModes => new[]
    {
        LearningMode.Reading,
        LearningMode.Listening,
        LearningMode.Speaking,
    };

    public LearningObjectType LearningObjectType => LearningItemType.Phrase.ToObjectType();
    public PhraseId CreateId(LearningObjectId id) =>
        new PhraseId(id.Value);

    public EntityId[] GetChildLearningItemIds(Phrase entity) =>
        ArrayExtensions.CreateArray<EntityId>();

    public Type EntityType => typeof(Phrase);
    public Type IdType => typeof(PhraseId);
}
