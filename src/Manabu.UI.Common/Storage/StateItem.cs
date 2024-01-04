using System.Reflection;

namespace Manabu.UI.Common.Storage;

public abstract class StateItem<T> : IStateItem
{
    private readonly IStorage _storage;

    public event Func<T, Task> OnValueChanged;
    public bool IsSet { get; private set; }

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
        IsSet = true;

        if (OnValueChanged is not null)
            await OnValueChanged?.Invoke(_value);
    }

    public async Task Init()
    {
        var type = GetType();
        var @object = await _storage.Get(type);
        if (@object is null)
            return;

        _value = (T) type
            .GetField(nameof(_value), BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(@object);

        IsSet = true;
    }
}

public interface IStateItem
{
    Task Init();
}
