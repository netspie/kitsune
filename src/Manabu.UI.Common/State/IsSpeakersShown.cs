using Manabu.UI.Common.Storage;

namespace Manabu.UI.Common.State;

public class IsSpeakersShown(IStorage storage) : StateItem<bool>(storage);