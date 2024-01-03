using Manabu.UI.Common.Storage;

namespace Manabu.UI.Common.State;

public class IsTranslationShown(IStorage storage) : StateItem<bool>(storage);