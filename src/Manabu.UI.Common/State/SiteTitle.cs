using Corelibs.Basic.Collections;

namespace Manabu.UI.Common.State;

public class SiteTitle
{
    public string Name { get; private set; }

    private HashSet<Action> _actions = new();

    public void Subscribe(Action action)
    {
        if (!_actions.Contains(action))
            _actions.Add(action);
    }

    public void Rename(string name)
    {
        Name = name;
        _actions.ForEach(a => a?.Invoke());
    }
}
