using Corelibs.Basic.DDD;
using Manabu.Entities.Content.Conversations;
using Manabu.Entities.Content.Courses;
using Manabu.Entities.Content.Infos;
using Manabu.Entities.Content.Phrases;
using Manabu.Entities.Content.Words;
using Manabu.Entities.Shared;

namespace Manabu.Entities.Content.Containers;

public class LearningContainer : Entity<LearningContainerId>, IAggregateRoot<LearningContainerId>
{
    public static string DefaultCollectionName { get; } = "learningContainers";

    public LearningObjectType Type { get; set; }

    public string Name { get; set; }
    public string? Description { get; set; }

    public List<CourseId>? Courses { get; set; }
    public List<ConversationId>? Conversations { get; set; }
    public List<PhraseId>? Phrases { get; set; }
    public List<WordId>? Words { get; set; }
    public List<Article>? Articles { get; set; }

    public List<OrderItem>? OrderItems { get; set; }

    public LearningContainer(
        LearningContainerId id,
        LearningObjectType type,
        string name) : base(id)
    {
        Type = type;
        Name = name;
    }

    public record OrderItem(
        LearningObjectType ObjectType,
        int Index);

    public record Article(
        string Content,
        List<InfoId> Infos);
}

public class LearningContainerId : EntityId { public LearningContainerId(string value) : base(value) {} }
