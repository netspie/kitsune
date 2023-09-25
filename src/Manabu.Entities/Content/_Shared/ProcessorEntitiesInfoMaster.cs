using Corelibs.Basic.DDD;
using Manabu.Entities.Shared;

namespace Manabu.Entities.Content._Shared;

public class ProcessorEntitiesInfoMaster
{
    private Dictionary<string, IProcessorEntityInfo> _infosByEntityType = new();
    private Dictionary<string, IProcessorEntityInfo> _infosByIdType = new();

    public IProcessorEntityInfo Add(
        IProcessorEntityInfo info)
    {
        _infosByEntityType.Add(info.LearningObjectType.Value, info);
        _infosByIdType.Add(info.IdType.Name, info);

        return info;
    }

    public TProcessorInfo Add<TProcessorInfo>()
        where TProcessorInfo : IProcessorEntityInfo, new()
    {
        var info = new TProcessorInfo();

        Add(info);

        return info;
    }

    public IProcessorEntityInfo Get(LearningObjectType type)
    {
        return _infosByEntityType[type.Value];
    }

    public LearningObjectType GetType(EntityId id)
    {
        var type = id.GetType();
        return _infosByIdType[type.Name].LearningObjectType;
    }
}
