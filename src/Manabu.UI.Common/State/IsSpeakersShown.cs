using Manabu.UI.Common.Storage;

namespace Manabu.UI.Common.State;

public class IsSpeakersShown : StateItem<bool>
{
    public IsSpeakersShown(IStorage storage) : base(storage)
    {
    }
}
