using Blazored.LocalStorage;

namespace Manabu.UI.Common.Storage;

public class BlazoredJsonLocalStorage : IStorage
{
    private readonly ILocalStorageService _localStorage;

    public BlazoredJsonLocalStorage(
        ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task Save<T>(T @object)
    {
        var key = typeof(T).GetType().Name;
        await _localStorage.SetItemAsync(key, @object);
    }

    public async Task<T> Get<T>()
    {
        var key = typeof(T).GetType().Name;
        return await _localStorage.GetItemAsync<T>(key);
    }

    public async Task Delete<T>()
    {
        var key = typeof(T).GetType().Name;
        await _localStorage.RemoveItemAsync(key);
    }
}
