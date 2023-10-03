using Corelibs.Basic.DDD;

namespace Manabu.Entities.Content.Infos;

public class Info : Entity<InfoId>, IAggregateRoot<InfoId>
{
    public static string DefaultCollectionName { get; } = "infos";

    public string Content { get; private set; }
    public List<string> Questions { get; private set; }

    public Info(string content)
    {
        Content = content;
    }

    public Info(
        InfoId id,
        uint version,
        string content) : base(id, version)
    {
        Content = content;
    }
}

public class InfoId : EntityId { public InfoId(string value) : base(value) { } }
