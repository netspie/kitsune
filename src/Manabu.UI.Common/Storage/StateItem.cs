using System.Reflection;

namespace Manabu.UI.Common.Storage;

public abstract class StateItem<T> : IStateItem
{
    private readonly IStorage _storage;

    protected StateItem(IStorage storage)
    {
        _storage = storage;
    }

    protected T _value;

    public T Value
    {
        get => _value;
        set
        {
            _value = value;
            SetEditModeStored();
        }
    }

    protected async Task SetEditModeStored()
    {
        if (_storage is null)
            return;

        await _storage.Save(this, GetType());
    }

    public async Task Init()
    {
        var type = GetType();
        var @object = await _storage.Get(type);
        if (@object is null)
            return;

        _value = (T)type
            .GetField(nameof(_value), BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(@object);
    }
}

public interface IStateItem
{
    Task Init();
}
