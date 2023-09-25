using Manabu.Entities.Shared;

namespace Manabu.Entities.Content._Shared;

public class ProcessorEntitiesInfoMaster
{
    private Dictionary<string, IProcessorEntityInfo> _infos = new();

    public IProcessorEntityInfo Add(IProcessorEntityInfo info)
    {
        _infos.Add(info.LearningObjectType.Value, info);
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
        return _infos[type.Value];
    }
}
