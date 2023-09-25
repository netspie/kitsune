using Corelibs.Basic.Collections;
using Corelibs.Basic.DDD;
using Manabu.Entities.Content._Shared;
using Manabu.Entities.Shared;

namespace Manabu.Entities.Content.Conversations;

public class ConversationInfo : IProcessorEntityInfo<Conversation, ConversationId>
{
    public bool IsLearningItem => false;
    public LearningMode[] LearningModes => Array.Empty<LearningMode>();
    public LearningObjectType LearningObjectType => LearningContainerType.Conversation;
    public ConversationId CreateId(LearningObjectId id) =>
        new ConversationId(id.Value);

    public EntityId[] GetChildLearningItemIds(Conversation entity) =>
        ArrayExtensions.CreateArray<EntityId>(
            entity.Phrases.Select(p => p.Phrase));

    public Type EntityType => typeof(Conversation);
    public Type IdType => typeof(ConversationId);
}
