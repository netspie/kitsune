using Manabu.UI.Common.Storage;

namespace Manabu.UI.Common.State;

public class IsTranslationShown : StateItem<bool>
{
    public IsTranslationShown(IStorage storage) : base(storage)
    {
    }
}
